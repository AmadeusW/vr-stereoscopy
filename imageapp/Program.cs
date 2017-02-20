using ImageSharp;
using System;

namespace imageapp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                throw new ArgumentException("First argument must be the file path");
            }
            var path = args[0];
            Console.WriteLine($"Processing {path}");

            using (Image image = new Image(path))
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

                imageL.Save("left.jpg");
                imageR.Save("right.jpg");
            }
        }

        private static int findMargin(Image<Color> image, int searchX = 0, int searchY = 0)
        {
            if (searchX == 0 && searchY == 0)
            {
                throw new ArgumentException("Either searchX or searchY must be non-zero.");
            }
            if (searchX != 0 && searchY != 0)
            {
                throw new ArgumentException("Only one of searchX and searchY can be non-zero.");
            }
            // Find starting point
            Point searchPoint;
            if (searchX > 0)
            {
                searchPoint = new Point(0, image.Height / 2);
            }
            else if (searchX < 0)
            {
                searchPoint = new Point(image.Width, image.Height / 2);
            }
            else if (searchY > 0)
            {
                searchPoint = new Point(image.Width/ 2, 0);
            }
            else // if (searchY < 0)
            {
                searchPoint = new Point(image.Width / 2, image.Height);
            }

            using (var pixels = image.Lock())
            {
                Color lastColor = pixels[0, 0];

                // Traverse into the picture. Stop when color differs
                int verifyLength = searchY == 0 ? image.Height : image.Width; // If first pass was horizontal, traverse vertically
                for (int i = 0; i < verifyLength; i++)
                {
                    Console.WriteLine($"searching at {searchPoint}");
                    var color = pixels[searchPoint.X, searchPoint.Y];
                    if (!AlmostEquals(color, lastColor))
                    {
                        Console.WriteLine($"First pass: stopping at {searchPoint}. Color was {color} but expected {lastColor}");
                        return i;
                    }
                    lastColor = color;
                    searchPoint.X += searchX;
                    searchPoint.Y += searchY;
                }
                return 0; // in doubt, don't cut
            }
        }

        public static bool AlmostEquals(Color a, Color b)
        {
            if (a.R - b.R > 1 || a.R - b.R < -1) return false;
            if (a.G - b.G > 1 || a.G - b.G < -1) return false;
            if (a.B - b.B > 1 || a.B - b.B < -1) return false;
            return true;
        }
    }
}
