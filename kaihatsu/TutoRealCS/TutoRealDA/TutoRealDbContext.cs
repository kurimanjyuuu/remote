using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TutoRealDA
{
    public class TutoRealDbContext : DbContext
    {
        public TutoRealDbContext(DbContextOptions<TutoRealDbContext> options) : base(options)
        {

        }
    }
}
