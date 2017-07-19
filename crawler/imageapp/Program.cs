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
                ProcessCrossViewFile(file);
            }
        }

        public static ImageProperties ProcessCrossViewFile(string file)
        {
            var filename = Path.GetFileNameWithoutExtension(file);
            Console.Write($"Processing {filename}: ");
            using (Image<Rgba32> image = Image.Load(file))
            {
                var result = ProcessCrossViewImage(image, filename);
                return result;
            }
        }

        public static ImageProperties ProcessPair(string file1, string file2, string name)
        {
            Console.Write($"Processing {name}: ");
            using (Image<Rgba32> image1 = Image.Load(file1))
            using (Image<Rgba32> image2 = Image.Load(file2))
            {
                return Work(image1, image2, name);
            }
        }

        private static ImageProperties ProcessCrossViewImage(Image<Rgba32> image, string filename)
        {
            // In cross eyed images, the image for the left eye is on the right side and vice versa.
            var imageR = new Image<Rgba32>(image).Crop(new Rectangle(0, 0, image.Width / 2, image.Height));
            var imageL = new Image<Rgba32>(image).Crop(new Rectangle(image.Width / 2, 0, image.Width / 2, image.Height));
            //return Work(imageL, imageR, filename); // Old stereograms don't switch left and right, so revert images back to normal
            return Work(imageR, imageL, filename);
        }

        private static ImageProperties Work(Image<Rgba32> imageR, Image<Rgba32> imageL, string filename)
        {
            Console.Write("u");
            var cropUp = findMargin(imageL, searchY: 1);
            Console.Write("d");
            var cropDown = findMargin(imageL, searchY: -1);
            Console.Write("l");
            var crop1Left = findMargin(imageL, searchX: 1);
            Console.Write("r");
            var crop1Right = findMargin(imageR, searchX: -1);
            Console.Write("l");
            var crop2Left = findMargin(imageL, searchX: 1);
            Console.Write("r");
            var crop2Right = findMargin(imageR, searchX: -1);

            Console.Write("C");
            imageL = imageL.Crop(new Rectangle(crop1Left, cropUp, imageL.Width - crop1Left - crop1Right, imageL.Height - cropUp - cropDown));
            Console.Write("C");
            imageR = imageR.Crop(new Rectangle(crop1Left, cropUp, imageR.Width - crop2Left - crop2Right, imageR.Height - cropUp - cropDown));

            // Get thumbnails
            Console.Write("t");
            var thumbnailL = new Image<Rgba32>(imageL)
                .Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(128, 128) })
                .Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(128, 128) });
            Console.Write("t");
            var thumbnailR = new Image<Rgba32>(imageR)
                .Resize(new ResizeOptions { Mode = ResizeMode.Max, Size = new Size(128, 128) })
                .Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(128, 128) });
            Console.Write("w");
            using (var stream = new FileStream($"out/{filename}.T.L.jpg", FileMode.Create))
            {
                thumbnailL.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 80 });
            }
            Console.Write("w");
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
            Console.Write("R");
            imageL = imageL.Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(newWidth, newHeight) });
            Console.Write("R");
            imageR = imageR.Resize(new ResizeOptions { Mode = ResizeMode.BoxPad, Size = new Size(newWidth, newHeight) });

            Console.Write("W");
            using (var stream = new FileStream($"out/{filename}.L.jpg", FileMode.Create))
            {
                imageL.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 100 });
            }
            Console.Write("W");
            using (var stream = new FileStream($"out/{filename}.R.jpg", FileMode.Create))
            {
                imageR.SaveAsJpeg(stream, new JpegEncoderOptions { Quality = 100 });
            }
            Console.WriteLine(" :)");
            return new ImageProperties()
            {
                Width = (int)Math.Log(newWidth, 2),
                Height = (int)Math.Log(newHeight, 2),
            };
        }

        private static int findMargin(Image<Rgba32> image, int searchX = 0, int searchY = 0)
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
                    var firstColor = pixels[point.X, point.Y];

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

        public static bool AlmostEquals(Rgba32 a, Rgba32 b)
        {
            if (a.R - b.R > 9 || a.R - b.R < -9) return false;
            if (a.G - b.G > 9 || a.G - b.G < -9) return false;
            if (a.B - b.B > 9 || a.B - b.B < -9) return false;
            return true;
        }
    }
}
