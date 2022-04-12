using QuestionReaction.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data
{
    public class HashService : IHashService
    {
        public string HashPassword(string clearPwd)
        {
            if (string.IsNullOrEmpty(clearPwd))
                return string.Empty;

            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = Encoding.ASCII.GetBytes(clearPwd);
            using (var sha = SHA256.Create())
            {
                tmpHash = sha.ComputeHash(tmpSource);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < tmpHash.Length; i++)
                {
                    sb.Append(tmpHash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
