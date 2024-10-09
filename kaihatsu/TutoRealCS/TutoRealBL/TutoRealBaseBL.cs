using TutoRealBE;
using TutoRealBE.Context;
using TutoRealDA;
using TutoRealIF;
using Microsoft.Extensions.Configuration;
using TutoRealBL.Master;
using Microsoft.AspNetCore.Http;
using TutoRealBE.Result;
using Microsoft.AspNetCore.Mvc;

namespace TutoRealBL
{
    public class TutoRealBaseBL : ITutoRealBaseIF
    {
        // ここでIConfigurationとTutoRealDbContextのインスタンスを取得するためのメソッドまたはプロパティが必要です。
        private readonly TutoRealDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public TutoRealBaseBL(TutoRealDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public Task<IEnumerable<ParentContext>> Select(ParentContext context)
        {
            return SelectImpl(context);
        }

        public Task<IEnumerable<ParentContext>> Insert(ParentContext context)
        {
            return InsertImpl(context);
        }

        public Task<IEnumerable<ParentContext>> Update(ParentContext context)
        {
            return UpdateImpl(context);
        }

        public Task<IEnumerable<ParentContext>> Delete(ParentContext context)
        {
            return DeleteImpl(context);
        }

        public Task<IEnumerable<ParentContext>> FileSave(ParentContext context)
        {
            return InsertImpl(context);
        }

        private async Task<IEnumerable<ParentContext>> SelectImpl(ParentContext context)
        {
            return context switch
            {
                LoginContext login => await new LoginBL(_dbContext, _configuration).SelectAsync(login),
                StandByConditionContext cond => await new StandByBL(_dbContext, _configuration).SelectAsync(cond),
                BookConditionContext cond => await new BookBL(_dbContext, _configuration).SelectAsync(cond),
                BaseContext parent => await new MasterBL(_dbContext, _configuration).SelectAsync(parent),
                BookGetContext parent => await new BookBL(_dbContext, _configuration).GetAsync(parent),
                EmpInfoGetContext parent => await new EmpBL(_dbContext, _configuration).SelectAsync(parent),
                _ => throw new ArgumentException("未対応のコンテキストです。", nameof(context))
            };
        }
        private async Task<IEnumerable<ParentContext>> InsertImpl(ParentContext context)
        {
            return context switch
            {
                BookRegistContext bookregist => await new BookBL(_dbContext, _configuration).InsertAsync(bookregist),
                EmpInfoGetContext empregist => await new EmpBL(_dbContext, _configuration).InsertAsync(empregist),
                _ => throw new ArgumentException("未対応のコンテキストです。", nameof(context))
            };
        }
        private async Task<IEnumerable<ParentContext>> UpdateImpl(ParentContext context)
        {
            return context switch
            {
                BookRegistContext bookregist => await new BookBL(_dbContext, _configuration).UpdateAsync(bookregist),
                BookLendReturnContext lendreturn => await new BookBL(_dbContext, _configuration).LendReturnAsync(lendreturn),
                EmpInfoGetContext empregist => await new EmpBL(_dbContext, _configuration).UpdateAsync(empregist),
                _ => throw new ArgumentException("未対応のコンテキストです。", nameof(context))
            };
        }
        private async Task<IEnumerable<ParentContext>> DeleteImpl(ParentContext context)
        {
            return context switch
            {
                EmpInfoGetContext parent => await new EmpBL(_dbContext, _configuration).DeleteAsync(parent),
                _ => throw new ArgumentException("未対応のコンテキストです。", nameof(context))
            };
        } 
    }
}
