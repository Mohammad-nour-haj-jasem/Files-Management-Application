using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files.Domain.ViewModels
{
    public class FileUploadForCreation
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string CreatedBy { get; set; }
    }
}
