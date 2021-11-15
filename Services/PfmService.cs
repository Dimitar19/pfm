using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Database.Repositories;
using pfm.Models;

namespace pfm.Services
{
    public class PfmService : IPfmService
    {
        private readonly IPfmRepository _pfmRepository;
        private readonly IMapper _mapper;
        public PfmService(IPfmRepository pfmRepository, IMapper mapper)
        {
            _pfmRepository = pfmRepository;
            _mapper = mapper;
        }
        public async Task<TransactionPagedList<Transaction>> GetTransactions(TransactionKind? transactionKind, DateTime? startDate, DateTime? endDate, int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var pagedSortedList = await _pfmRepository.Get(transactionKind, startDate, endDate, page, pageSize, sortBy, sortOrder);

            return _mapper.Map<TransactionPagedList<Transaction>>(pagedSortedList);
        }
        public async Task<List<Category>> GetCategories(string parentId)
        {
            var categoriesList = await _pfmRepository.Get(parentId);
            /*return categoriesList.Select(c=> new Category{
                Code = c.Code,
                Name = c.Name,
                //ParentCode = c.ParentCode,
            }).ToList();*/
            var mapped = _mapper.Map<List<Category>>(categoriesList);
            //return _mapper.Map<List<Category>>(categoriesList);
            return mapped;
        }

        public async Task<Transaction> ImportTransactions(IFormFile file)
        {
            string [] colFields;
            string [] pls;
            CreateTransactionCommand pom = new CreateTransactionCommand();//temp

            using(TextFieldParser csvReader = new TextFieldParser(file.OpenReadStream()))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;
                colFields = csvReader.ReadFields();
                pls = csvReader.ReadFields();

                for(int i = 0; i < 9; ++i)//temp
                    pls = csvReader.ReadFields();//temp

                pom.Id = pls[0];//temp
                pom.BeneficiaryName = pls[1];//temp
                pom.Date = DateTime.Parse(pls[2]);//temp
                pom.Direction = (Directions)Enum.Parse(typeof(Directions), pls[3], true);//temp
                pom.Amount = Double.Parse(pls[4].Substring(2));//temp
                pom.Description = pls[5];//temp
                pom.Currency = pls[6];//temp
                pom.Kind = (TransactionKind)Enum.Parse(typeof(TransactionKind), pls[8], true);//temp

                /*while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();

                    var check = await _pfmRepository.TransactionGet(fieldData[0]);
                    if (check != null)
                        continue;
                    
                    CreateTransactionCommand transaction = new CreateTransactionCommand();

                    transaction.Id = fieldData[0];
                    transaction.BeneficiaryName = fieldData[1];
                    transaction.Date = DateTime.Parse(fieldData[2]);
                    transaction.Direction = (Directions)Enum.Parse(typeof(Directions), fieldData[3], true);
                    transaction.Amount = Double.Parse(fieldData[4].Substring(2));
                    transaction.Description = fieldData[5];
                    transaction.Currency = fieldData[6];
                    transaction.mcc = null;
                    transaction.Kind = (TransactionKind)Enum.Parse(typeof(TransactionKind), fieldData[8], true);

                    var createProduct = _mapper.Map<TransactionEntity>(transaction);
                    await _pfmRepository.CreateTransaction(createProduct);
                }*/
            }

            var check = await _pfmRepository.TransactionGet(pom.Id);//temp
            if (check != null)//temp
                return _mapper.Map<Transaction>(pom);//temp

            var createTransaction = _mapper.Map<TransactionEntity>(pom);//temp
            var res = await _pfmRepository.CreateTransaction(createTransaction);//temp
            var transactionCreate = _mapper.Map<Transaction>(res);//temp
            return transactionCreate;//temp
        }

        public async Task<Category> ImportCategories(IFormFile file)
        {
            string [] colFields;
            string [] pls;
            CreateCategoryCommand pom = new CreateCategoryCommand();//temp

            using(TextFieldParser csvReader = new TextFieldParser(file.OpenReadStream()))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = false;
                colFields = csvReader.ReadFields();
                pls = csvReader.ReadFields();

                for(int i = 0; i < 22; ++i)//temp
                    pls = csvReader.ReadFields();//temp

                pom.Code = pls[0];//temp
                pom.Name = pls[2];//temp
                if (string.IsNullOrEmpty(pls[1]))//temp
                    pom.ParentCode = null;//temp
                else//temp
                    pom.ParentCode = pls[1];//temp

                /*while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();

                    var check = await _pfmRepository.CategoryGet(fieldData[0]);
                    if (check != null)
                        continue;
                    
                    CreateCategoryCommand category = new CreateCategoryCommand();

                    category.Code = fieldData[0];
                    category.Name = fieldData[1];
                    category.ParentCode = fieldData[2];

                    var createProduct = _mapper.Map<CategoryEntity>(category);
                    await _pfmRepository.CreateCategory(createProduct);
                }*/
            }

            var check = await _pfmRepository.CategoryGet(pom.Code);//temp
            if (check != null)//temp
                return _mapper.Map<Category>(pom);//temp

            var createCategory = _mapper.Map<CategoryEntity>(pom);//temp
            var res = await _pfmRepository.CreateCategory(createCategory);//temp
            var categoryCreate = _mapper.Map<Category>(res);//temp
            return categoryCreate;//temp
        }

        public async Task<Transaction> CategorizeTransaction(string id, TransactionCategorizeCommand command){
            var res = await _pfmRepository.UpdateCategory(id, command);
            return _mapper.Map<Transaction>(res);
        }

        public async Task<SpendingsByCategory> GetSpendingAnalytics(string catCode, DateTime? startDate, DateTime? endDate, Directions? direction)
        {
            var spendingAnalytics = await _pfmRepository.GetSpendingAnalytics(catCode, startDate, endDate, direction);
            return spendingAnalytics;
        }
    }
}