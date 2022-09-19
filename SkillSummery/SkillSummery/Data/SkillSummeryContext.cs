using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace SkillSummery.Data
{
    public class SkillSummeryContext : DbContext
    {
        public SkillSummeryContext (DbContextOptions<SkillSummeryContext> options)
            : base(options)
        {
        }

        public DbSet<MvcMovie.Models.Movie> Movie { get; set; } = default!;
    }
}
