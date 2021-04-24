using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;

namespace BookMan.Models
{
    public class Service
    {
        private readonly string _dataFile = @"Data\data.xml";
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(HashSet<Book>));
        public HashSet<Book> Books { get; set; }

        public Service()
        {
            if (!File.Exists(_dataFile))
            {
                Books = new HashSet<Book>()
                {
                    new Book
                    {
                        Id=1,
                        Name="ASP.NET Core for dummy",
                        Authors="Trump D",
                        Publisher="Washington",
                        Year=2020
                    },
                    new Book
                    {
                        Id=2,
                        Name="ASP.NET Core for dummy2",
                        Authors="Trump D2",
                        Publisher="Washington2",
                        Year=2020
                    },
                    new Book
                    {
                        Id=3,
                        Name="ASP.NET Core for dummy3",
                        Authors="Trump D3",
                        Publisher="Washington3",
                        Year=2020
                    },
                    new Book
                    {
                        Id=4,
                        Name="ASP.NET Core for dummy3",
                        Authors="Trump D3",
                        Publisher="Washington3",
                        Year=2020
                    },
                    new Book
                    {
                        Id=5,
                        Name="ASP.NET Core for dummy3",
                        Authors="Trump D3",
                        Publisher="Washington3",
                        Year=2020
                    }
                };
            }
            else
            {
                using(var stream = File.OpenRead(_dataFile))
                {
                    Books = _serializer.Deserialize(stream) as HashSet<Book>;
                }
            }
        }


        public Book[] Get() => Books.ToArray();

        public Book Get(int id) => Books.FirstOrDefault(x => x.Id == id);

        public bool Add(Book book) => Books.Add(book);

        public Book Create()
        {
            var max = Books.Max(x => x.Id);
            var b = new Book
            {
                Id = max + 1,
                Year = DateTime.Now.Year
            };
            return b;
        }

        public bool Update(Book book)
        {
            var b = Get(book.Id);
            return b != null && Books.Remove(b) && Books.Add(book);
        }

        public bool Delete(int id)
        {
            var b = Get(id);
            return b != null && Books.Remove(b);
        }

        public string GetPathData(string file) => $"Data\\{file}";

        public void Upload(Book book,IFormFile file)
        {
            if (file != null)
            {
                var path = GetPathData(file.FileName);
                using var stream = new FileStream(path,FileMode.Create);
                file.CopyTo(stream);
                book.DataFile = file.FileName;
            }
        }

        public (Stream,string) Download(Book book)
        {
            var memory = new MemoryStream();
            using var stream = new FileStream(GetPathData(book.DataFile), FileMode.Open);
            stream.CopyTo(memory);
            memory.Position = 0;
            var type = Path.GetExtension(book.DataFile) switch
            {
                "pdf" => "application/pdf",
                "docx" => "application/vnd.ms-word",
                "doc" => "application/vnd.ms-word",
                "txt" => "text/plain",
                _ => "application/pdf"
            };
            return (memory, type);
        }

        public Book[] Get(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Books.ToArray();
            }
            var s = search.ToLower();
            return Books.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(s) ||
                                    !string.IsNullOrEmpty(x.Authors) && x.Authors.ToLower().Contains(s) ||
                                    !string.IsNullOrEmpty(x.Publisher) && x.Publisher.ToLower().Contains(s) ||
                                    !string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(s) ||
                                    x.Year.ToString().ToLower().Contains(s)).ToArray();
        }

        public (Book[] books,int pages,int page) Paging(int page)
        {
            int size = 5;
            int pages = (int)Math.Ceiling((double)Books.Count / size);
            var books = Books.Skip((page - 1) * size).Take(size).ToArray();
            return (books, pages, page);
        }

        public void SaveChange()
        {
            using var stream = File.Create(_dataFile);
            _serializer.Serialize(stream, Books);
        }
    }
}
