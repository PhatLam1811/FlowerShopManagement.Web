﻿using MongoDB.Bson.Serialization.Attributes;

namespace FlowerShopManagement.Core.Model
{
    internal class Account
    {
        string accountId = String.Empty;
        string accountUsername = String.Empty;
        string accountPassword = String.Empty;
        int accountType = 0;

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        public string AccountUsername
        {
            get { return accountUsername; }
            set { accountUsername = value; }
        }
        public string AccountPassword
        {
            get { return accountPassword; }
            set { accountPassword = value; }
        }
        public int AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }
    }
}
