using System;

namespace MyImages
{
    class Program
    {
        static void Main(string[] args)
        {
            var file1 = @"C:\Users\amadeusz\Pictures\Stereo\out\fence.jpg";
            var file2 = @"C:\Users\amadeusz\Pictures\Stereo\out\fenceL.jpg";
            StereoscopyVR.ImageApp.Program.ProcessPair(file1, file2);
        }
    }
}