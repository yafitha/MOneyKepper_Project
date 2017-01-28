using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoneyKepperServer
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<Category, Models.Category>();
            Mapper.CreateMap<Models.Category, Category>();
            Mapper.CreateMap<Transaction, Models.Transaction>();
            Mapper.CreateMap<TransactionType, Models.TransactionType>();
            Mapper.CreateMap<Models.Transaction, Transaction>();
            Mapper.CreateMap<Models.TransactionType, TransactionType>();
        }
    }

}