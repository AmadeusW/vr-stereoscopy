using ImageSharp;
using ImageSharp.Formats;
using ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StereoscopyVR.ImageApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                throw new ArgumentException("First argument must be the file path");
            }
            var path = args[0];
            var directory = Path.GetDirectoryName(path);
            Console.WriteLine($"Processing {directory}");
            Directory.CreateDirectory("out");
            foreach (var file in Directory.EnumerateFiles(directory, "*.jpg").Union(Directory.EnumerateFiles(directory, "*.png")))
            {
                ProcessFile(file);
            }
        }

        public static void ProcessFile(string file)
        {
            Console.WriteLine($"Processing {file}");
            var filename = Path.GetFileNameWithoutExtension(file);
            try
            {
                using (Image image = new Image(file))
                {
                    Work(image, filename);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error processing {file}: {e.Message}");
            }
        }

        private static void Work(Image image, string filename)
        {
            var imageL = new Image(image).Crop(new Rectangle(0, 0, image.Width / 2, image.Height));
            var imageR = new Image(image).Crop(new Rectangle(image.Width / 2, 0, image.Width / 2, image.Height));

            Console.WriteLine("cropUp");
            var cropUp = findMargin(imageL, searchY: 1);
            Console.WriteLine("cropDown");
            var cropDown = findMargin(imageL, searchY: -1);
            Console.WriteLine("crop1Left");
            var crop1Left = findMargin(imageL, searchX: 1);
            Console.WriteLine("crop1Right");
            var crop1Right = findMargin(imageR, searchX: -1);
            Console.WriteLine("crop2Left");
            var crop2Left = findMargin(imageL, searchX: 1);
            Console.WriteLine("crop2Right");
            var crop2Right = findMargin(imageR, searchX: -1);

            imageL = imageL.Crop(new Rectangle(crop1Left, cropUp, imageL.Width - crop1Left - crop1Right, imageL.Height - cropUp - cropDown));
            imageR = imageR.Crop(new Rectangle(crop1Left, cropUp, imageR.Width - crop2Left - crop2Right, imageR.Height - cropUp - cropDown));

            // Get thumbnails
            var thumbnailL = new Image(imageL as Image)
                .Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(128, 128) })
                .Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(128, 128) });
            var thumbnailR = new Image(imageR as Image)
                .Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(128, 128) })
                .Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(128, 128) });
            using (var stream = new FileStream($"out/{filename}.T.L.jpg", FileMode.Create))
            {
                thumbnailL.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 80 });
            }
            using (var stream = new FileStream($"out/{filename}.T.R.jpg", FileMode.Create))
            {
                thumbnailR.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 80 });
            }

            // If image size is greater than 1024, shrink it 
            // Increase size of images to a nearest power of 2
            var newWidth = (int)Math.Pow(2, Math.Ceiling(Math.Log(Math.Max(imageL.Width, imageR.Width), 2)));
            var newHeight = (int)Math.Pow(2, Math.Ceiling(Math.Log(Math.Max(imageL.Height, imageR.Height), 2)));
            newWidth = Math.Min(1024, newWidth);
            newHeight = Math.Min(1024, newHeight);
            imageL = imageL.Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(newWidth, newHeight) });
            imageR = imageR.Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(newWidth, newHeight) });

            using (var stream = new FileStream($"out/{filename}.L.jpg", FileMode.Create))
            {
                imageL.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 100 });
            }
            using (var stream = new FileStream($"out/{filename}.R.jpg", FileMode.Create))
            {
                imageR.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 100 });
            }
        }

        private static int findMargin(Image<Color> image, int searchX = 0, int searchY = 0)
        {
            Debug.WriteLine($"findMargin: {searchX}, {searchY}");
            if (searchX == 0 && searchY == 0)
            {
                throw new ArgumentException("Either searchX or searchY must be non-zero.");
            }
            if (searchX != 0 && searchY != 0)
            {
                throw new ArgumentException("Only one of searchX and searchY can be non-zero.");
            }
            // Find starting point
            Point[] searchPoints;
            if (searchX > 0)
            {
                searchPoints = new Point[] 
                {
                    new Point(0, image.Height / 4),
                    new Point(0, image.Height / 4 * 2),
                    new Point(0, image.Height / 4 * 3)
                };
            }
            else if (searchX < 0)
            {
                searchPoints = new Point[]
                {
                    new Point(image.Width - 1, image.Height / 4),
                    new Point(image.Width - 1, image.Height / 4 * 2),
                    new Point(image.Width - 1, image.Height / 4 * 3)
                };
            }
            else if (searchY > 0)
            {
                searchPoints = new Point[]
                {
                    new Point (image.Width/ 4, 0),
                    new Point (image.Width/ 4 * 2, 0),
                    new Point (image.Width/ 4 * 3, 0)
                };
            }
            else // if (searchY < 0)
            {
                searchPoints = new Point[]
                {
                    new Point(image.Width / 4, image.Height - 1),
                    new Point(image.Width / 4 * 2, image.Height - 1),
                    new Point(image.Width / 4 * 3, image.Height - 1)
                };
            }

            using (var pixels = image.Lock())
            {
                var distances = new List<int>();
                foreach (var point in searchPoints)
                {
                    var searchPoint = point;
                    Color firstColor = pixels[point.X, point.Y];

                    // Traverse into the picture. Stop when color differs
                    int verifyLength = searchY == 0 ? image.Height : image.Width; // If first pass was horizontal, traverse vertically
                    for (int i = 0; i < verifyLength; i++)
                    {
                        //Console.WriteLine($"searching at {searchPoint}");
                        var color = pixels[searchPoint.X, searchPoint.Y];
                        if (!AlmostEquals(color, firstColor))
                        {
                            Debug.WriteLine($"Candidate at {i}");
                            distances.Add(i);
                            break;
                        }
                        searchPoint.X += searchX;
                        searchPoint.Y += searchY;
                    }
                }
                Debug.WriteLine($"First pass: returning {distances.Min()}");
                return distances.Min();
            }
        }

        public static bool AlmostEquals(Color a, Color b)
        {
            if (a.R - b.R > 9 || a.R - b.R < -9) return false;
            if (a.G - b.G > 9 || a.G - b.G < -9) return false;
            if (a.B - b.B > 9 || a.B - b.B < -9) return false;
            return true;
        }
    }
}
