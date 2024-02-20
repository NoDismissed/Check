using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            string data = "\\data";
            string[] names = { "gif", "html", "jpg", "mp4", "swf" };
            string[] keys = { "\\gif", "\\html", "\\jpg", "\\mp4", "\\swf" };
            List<Read> keysList = new List<Read>();
            List<string> conceptList = new List<string>();
            int noGifCount = 0;
            int noJpgCount = 0;
            int noMp4Count = 0;
            List<string> diffExtensionList = new List<string>();
            int oversizeGifCount = 0;
            int oversizeJpgCount = 0;
            int oversizeMp4Count = 0;
            List<string> oversizeList = new List<string>();
            int gifMaxMb = 0;
            int jpgMaxKb = 0;
            int mp4MaxMb = 0;
            
            if (Directory.Exists(path))
            {
                if (Directory.Exists($"{path}{data}"))
                {
                    Console.WriteLine("gif max in mb: ");
                    while (!int.TryParse(Console.ReadLine(), out gifMaxMb))
                        Console.WriteLine("gif max in mb: ");

                    Console.WriteLine("jpg max in kb: ");
                    while (!int.TryParse(Console.ReadLine(), out jpgMaxKb))
                        Console.WriteLine("jpg max in kb: ");

                    Console.WriteLine("mp4 max in mb: ");
                    while (!int.TryParse(Console.ReadLine(), out mp4MaxMb))
                        Console.WriteLine("mp4 max in mb: ");

                    Console.Clear();

                    foreach (string item in keys)
                    {
                        if (Directory.Exists($"{path}{data}{item}"))
                        {
                            keysList.Add(new Read
                            {
                                folders = Directory.GetDirectories($"{path}{data}{item}").Length,
                                items = Directory.GetFiles($"{path}{data}{item}", "*.*", SearchOption.AllDirectories).Count()
                            });

                            var concepts = Directory.GetDirectories($"{path}{data}{item}").Select(Path.GetFileName).ToList();
                            if (concepts.Count() > 0)
                            {
                                for (int concept = 0; concept < concepts.Count(); concept++)
                                    conceptList.Add(concepts[concept]);
                            }

                            if (item == "\\gif")
                            {
                                var files = Directory.GetFiles($"{path}{data}{item}", "*.*", SearchOption.AllDirectories).ToList();

                                var not = files.Where(x => !x.EndsWith(".gif")).ToList();
                                noGifCount = not.Count();
                                if (noGifCount > 0)
                                {
                                    for (int file = 0; file < noGifCount; file++)
                                        diffExtensionList.Add(not[file]);
                                }

                                if (files.Count() > 0)
                                {
                                    int count = 0;
                                    foreach (string file in files)
                                    {
                                        FileInfo info = new FileInfo(file);
                                        if (info.Length >= (gifMaxMb * 1048576))
                                        {
                                            decimal length = info.Length;
                                            oversizeList.Add($"{info.FullName} ({Math.Round(length / 1048576, 2)} mb)");
                                            count++;
                                        }
                                    }
                                    oversizeGifCount = count;
                                }
                            }

                            if (item == "\\jpg")
                            {
                                var files = Directory.GetFiles($"{path}{data}{item}", "*.*", SearchOption.AllDirectories).ToList();

                                var not = files.Where(x => !x.EndsWith(".jpg")).ToList();
                                noJpgCount = not.Count();
                                if (noJpgCount > 0)
                                {
                                    for (int file = 0; file < noJpgCount; file++)
                                        diffExtensionList.Add(not[file]);
                                }

                                if (files.Count() > 0)
                                {
                                    int count = 0;
                                    foreach (string file in files)
                                    {
                                        FileInfo info = new FileInfo(file);
                                        if (info.Length >= (jpgMaxKb * 1024))
                                        {
                                            decimal length = info.Length;
                                            oversizeList.Add($"{info.FullName} ({Math.Round(length / 1024, 2)} kb)");
                                            count++;
                                        }
                                    }
                                    oversizeJpgCount = count;
                                }
                            }

                            if (item == "\\mp4")
                            {
                                var files = Directory.GetFiles($"{path}{data}{item}", "*.*", SearchOption.AllDirectories).ToList();

                                var not = files.Where(x => !x.EndsWith(".mp4")).ToList();
                                noMp4Count = not.Count();
                                if (noMp4Count > 0)
                                {
                                    for (int file = 0; file < noMp4Count; file++)
                                        diffExtensionList.Add(not[file]);
                                }

                                if (files.Count() > 0)
                                {
                                    int count = 0;
                                    foreach (string file in files)
                                    {
                                        FileInfo info = new FileInfo(file);
                                        if (info.Length >= (mp4MaxMb * 1048576))
                                        {
                                            decimal length = info.Length;
                                            oversizeList.Add($"{info.FullName} ({Math.Round(length / 1048576, 2)} mb)");
                                            count++;
                                        }
                                    }
                                    oversizeMp4Count = count;
                                }
                            }
                        }
                        else
                            Console.WriteLine($"{item} folder not found");
                    }

                    conceptList = conceptList.Distinct().OrderBy(x => x).ToList();

                    for (int item = 0; item < keys.Count(); item++)
                        Console.WriteLine($"{names[item]} - {keysList[item].folders} key - {keysList[item].items} items");

                    Console.WriteLine("");

                    for (int item = 0; item < conceptList.Count(); item++)
                        Console.WriteLine($"{item + 1}. {conceptList[item]}");

                    Console.WriteLine("");
                    Console.WriteLine($"{noGifCount} files not gif");
                    Console.WriteLine($"{noJpgCount} files not jpg");
                    Console.WriteLine($"{noMp4Count} files not mp4");
                    Console.WriteLine("");

                    for (int item = 0; item < diffExtensionList.Count(); item++)
                        Console.WriteLine(diffExtensionList[item]);

                    Console.WriteLine("");
                    Console.WriteLine($"{oversizeGifCount} gif files >= {gifMaxMb} mb");
                    Console.WriteLine($"{oversizeJpgCount} jpg files >= {jpgMaxKb} kb");
                    Console.WriteLine($"{oversizeMp4Count} mp4 files >= {mp4MaxMb} mb");
                    Console.WriteLine("");

                    for (int item = 0; item < oversizeList.Count(); item++)
                        Console.WriteLine(oversizeList[item]);
                }
                else
                    Console.WriteLine("data folder not found");
            }

            Console.ReadLine();
        }

        class Read
        {
            public int folders { get; set; }
            public int items { get; set; }
        }
    }
}
