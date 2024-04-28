using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files.Domain.Models
{
    public class FileUploade
    {
        public FileUploade()
        {
            this.Id= Guid.NewGuid();
            this.CreatedOn= DateTime.Now;
        }
        public Guid Id { get; set; }
        public  string Name { get; set; }
        public  string Path { get; set; }
        public   string FileType { get; set; }
        public  string Description { get; set; }
        public  DateTime CreatedOn { get; set; }
        public  string CreatedBy { get; set; }
    }

}
