using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    public class Chain
    {
        public List<Block> Blocks { get; private set; }

        public Block Last { get; private set;}
        public Chain()
        {
            Blocks = LoadChainFromDB();
            if (Blocks.Count == 0)
            {
                var genesisBlock = new Block();

                Blocks.Add(genesisBlock);
                Last = genesisBlock;
                Save(Last);
            }
            else
            {
                Last = Blocks.Last();
            }
        }

        public void Add(string data, string user)
        {
            var block = new Block(data, user, Last);
            Blocks.Add(block);
            Last = block;
            Save(Last);
        }
        public bool Check()
        {
            bool check = false;
            for (int i = 0; i < Blocks.Count-1; i++)
            {
                string data1 = this.Blocks[i].GetData();
                string hash1 = Block.GetHash(data1);
                if(Blocks[i+1].PreviousHash == hash1)
                {
                    check = true;
                }
                else
                {
                    check = false;
                    break;
                }
            }
            return check;
            
        }

        private void Save(Block block)
        {
            using(var db = new BlockchainContext())
            {
                db.Blocks.Add(block);
                db.SaveChanges();
            }
        }
        private List<Block> LoadChainFromDB()
        {
            List<Block> result;
            using (var db = new BlockchainContext())
            {
                var count = db.Blocks.Count();
                result = new List<Block>(count);

                result.AddRange(db.Blocks);
            }
            return result;
        }
    }
}
