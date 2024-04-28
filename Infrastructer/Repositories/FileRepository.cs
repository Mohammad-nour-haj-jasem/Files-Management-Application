using Files.Domain.Models;
using Infrastructer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.Repositories
{
    public class FileRepository : Repository<FileUploade>
    {
        public FileRepository(FileContext _context) : base(_context)
        {
        }
    }
}
