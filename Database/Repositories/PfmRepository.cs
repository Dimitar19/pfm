using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using pfm.Commands;
using pfm.Database.Entities;
using pfm.Models;

namespace pfm.Database.Repositories
{
    public class PfmRepository : IPfmRepository
    {
        private readonly PfmDbContext _dbContext;
        public PfmRepository(PfmDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TransactionEntity> TransactionGet(string id){
            var result = await _dbContext.Transactions.FirstOrDefaultAsync(t => t.Id.Equals(id));
            return result;
        }
        public async Task<CategoryEntity> CategoryGet(string code){
            var result = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Code.Equals(code));
            return result;
        }

        public async Task<TransactionPagedList<TransactionEntity>> Get(int page = 1, int pageSize = 10, string sortBy = null, SortOrder sortOrder = SortOrder.Asc)
        {
            var query = _dbContext.Transactions.AsQueryable();

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total * 1.0 / pageSize);

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortOrder == SortOrder.Desc)
                {
                    query = query.OrderByDescending(sortBy, p => p.Id);
                }
                else
                {
                    query = query.OrderBy(sortBy, p => p.Id);
                }
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new TransactionPagedList<TransactionEntity>
            {
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortOrder = sortOrder,
                TotalCount = total,
                TotalPages = totalPages,
                Items = await query.ToListAsync(),
            };
        }

        public async Task<List<CategoryEntity>> Get(string parentId)
        {
            /*var query = _dbContext.Categories.AsQueryable();
            var res = await query.ToListAsync();
            return res;*/
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<TransactionEntity> CreateTransaction(TransactionEntity transaction)
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }
        public async Task<CategoryEntity> CreateCategory(CategoryEntity category){
            // if (string.IsNullOrEmpty(category.ParentCode))
            // {
            //     category.ParentCode = null;
            //     category.ParentCategory = null;
            // }
            // else 
            // {
            //     category.ParentCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Code.Equals(category.ParentCode));
            // }
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public TransactionEntity UpdateCategory(string id, TransactionCategorizeCommand command)
        {
            // var pom = _dbContext.Transactions.FirstOrDefault(t => t.Id == id);
            // if (pom != null) {
            //     pom.CatCode = command.CatCode;
            //     pom.Category = CategoryGet(command.CatCode);
            //     var res = _dbContext.Transactions.Update(pom);
            //     _dbContext.SaveChanges();
            // }
            return (TransactionEntity) _dbContext.Transactions.FirstOrDefault(t => t.Id == id);
        }
    }
}