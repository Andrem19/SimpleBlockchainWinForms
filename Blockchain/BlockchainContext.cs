using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    class BlockchainContext : DbContext
    {
        public BlockchainContext() : base("Blockchain") { }

        public DbSet<Block> Blocks { get; set; }
    }
}
