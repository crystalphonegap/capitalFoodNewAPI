using ICICI_BANK_INTERFACE.Models;
using Microsoft.EntityFrameworkCore;

namespace ICICI_BANK_INTERFACE.Context
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        
    }
}
