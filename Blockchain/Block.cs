using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    [DataContract]
    public class Block
    {
        public int Id { get; set; }
        [DataMember]
        public string Data { get; private set; }
        [DataMember]
        public DateTime Created { get; private set; }
        [DataMember]
        public string Hash { get; private set; }
        [DataMember]
        public string PreviousHash { get; private set; }
        [DataMember]
        public string User { get; set; }

        public Block()
        {
            Id = 1;
            Data = "Hello World";
            Created = DateTime.Parse("01.09.19 00:00:00.000");
            PreviousHash = "111111111111111";
            User = "Admin";

            var data = GetData();
            Hash = GetHash(data);
        }
        public Block(string data, string user, Block block)
        {
            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException($"Emty document data", nameof(data));
            }

            if(block == null)
            {
                throw new ArgumentNullException($"Emty document data", nameof(block));
            }
            if (string.IsNullOrEmpty(user))
            {
                throw new ArgumentNullException($"Emty document data", nameof(user));
            }

            Data = data;
            User = user;
            PreviousHash = block.Hash;
            Created = DateTime.UtcNow;
            Id = block.Id + 1;
            var blockData = GetData();
            Hash = GetHash(blockData);

        }

        public string GetData()
        {
            string result = "";

            result += Data;
            result += Created.ToString("dd.MM.yyy HH:mm:ss");
            result += PreviousHash;
            result += User;
            return result;
        }

        public static string GetHash(string data)
        {
            var message = Encoding.ASCII.GetBytes(data);
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);

            foreach(byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }

            return hex;
        }

        public override string ToString()
        {
            return Data;
        }
        public string Serialize()
        {

            var jsonSerializer = new DataContractJsonSerializer(typeof(Block));

            using (var ms = new MemoryStream())
            {
                jsonSerializer.WriteObject(ms, this);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        public static Block Deserialize(string json)
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(Block));
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (Block)jsonSerializer.ReadObject(ms);

            }
        }
    }
}
