using System.Linq;
using System.Threading.Tasks;
using TutoRealDA;
using TutoRealBE.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Text;
using static TutoRealCommon.CommonConst;
using static TutoRealCommon.CommonFunctionLibrary;
using System.Data;
using TutoRealBE;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Common;
using TutoRealBE.Result;
using TutoRealBE.Entity;
using System.Collections.Generic;

namespace TutoRealDA.Master
{
    public class MasterDA : TutoRealBaseDA
    {
        private readonly IDbConnection _dbConnection;

        public MasterDA(IDbConnection connection, IConfiguration configuration) : base(connection, configuration)
        {
            _dbConnection = connection;

        }

        public async Task<IEnumerable<MasterDataResult>> SelectAsync(ParentContext context)
        {
            //Masterリストデータ
            List<MasterDataResult> masterDatas = new List<MasterDataResult>();
            //Masterインスタンス
            MasterDataResult masterData = new MasterDataResult();

            //拠点マスター
            string query = DepositoryMasterSelectQuery();
            IEnumerable<DepositoryEntity> depositorymaster = await this.Select<DepositoryEntity>(query);
            masterData.DepositoryMaster.AddRange(NewDepositoryMaster(depositorymaster));

            //カテゴリマスター
            query = CategoryMasterSelectQuery();
            IEnumerable<CategoryItemsEntity> Categorymaster = await this.Select<CategoryItemsEntity>(query);
            masterData.CategoryMaster.AddRange(NewCategoryMaster(Categorymaster));

            masterDatas.Add(masterData);
            return masterDatas;
        }

        private static string DepositoryMasterSelectQuery()
        {
            StringBuilder query = new StringBuilder().AppendLine($"SELECT DM.Id,DM.BaseId,BM.BaseName");
            query.AppendLine(",  FM.FloorId,FM.FloorName");
            query.AppendLine(",  AM.AreaId,AM.AreaName");
            query.AppendLine(",  DTM.DetailId,DTM.DetailName");
            query.AppendLine($"FROM {TblSet(TBLNAME.M_DEPOSITORYMASTER)} DM");
            query.AppendLine($"INNER JOIN {TblSet(TBLNAME.M_BASEMASTER)}  BM");
            query.AppendLine("ON DM.BaseId = BM.BaseId");
            query.AppendLine($"INNER JOIN {TblSet(TBLNAME.M_FLOORMASTER)} FM");
            query.AppendLine("ON BM.BaseId = FM.BaseId");
            query.AppendLine("AND DM.FloorId = FM.FloorId");
            query.AppendLine($"INNER JOIN {TblSet(TBLNAME.M_AREAMASTER)} AM");
            query.AppendLine("ON DM.AreaId = AM.AreaId");
            query.AppendLine("AND FM.FloorId = AM.FloorId");
            query.AppendLine($"INNER JOIN {TblSet(TBLNAME.M_DETAILMASTER)} DTM");
            query.AppendLine("ON DM.DetailId = DTM.DetailId");
            query.AppendLine("AND AM.AreaId = DTM.AreaId");
            query.AppendLine("WHERE DM.DelFlg = '0' AND BM.DelFlg = '0' AND FM.DelFlg = '0' AND AM.DelFlg = '0' AND DTM.DelFlg = '0'");
            query.AppendLine("ORDER BY BM.BaseId,FM.FloorId,AM.FloorId,DTM.DetailId");
            return query.ToString();
        }
        private static List<DepositoryEntity> NewDepositoryMaster(IEnumerable<DepositoryEntity> result)
        {
            List<DepositoryEntity> masters = new List<DepositoryEntity>();
            foreach (var item in result)
            {
                DepositoryEntity master = new DepositoryEntity()
                {
                    Id = item.Id,
                    BaseId = item.BaseId,
                    BaseName = item.BaseName,
                    FloorId = item.FloorId,
                    FloorName = item.FloorName,
                    AreaId= item.AreaId,
                    AreaName=item.AreaName,
                    DetailId=item.DetailId,
                    DetailName = item.DetailName
                };
                masters.Add(master);
            }
            return masters;
        }

        private static string CategoryMasterSelectQuery() =>
            new StringBuilder().AppendLine($"SELECT Id,BitValue,Name FROM {TblSet(TBLNAME.M_CATEGORYMASTER)} WHERE DelFlg = '0'").ToString();
        private static List<CategoryItemsEntity> NewCategoryMaster(IEnumerable<CategoryItemsEntity> result)
        {
            List<CategoryItemsEntity> masters = new List<CategoryItemsEntity>();
            foreach (var item in result)
            {
                CategoryItemsEntity master = new CategoryItemsEntity()
                {
                    Id = item.Id,
                    Name =item.Name,
                    CheckStatus = false,
                    Bitvalue = item.Bitvalue
                };
                masters.Add(master);
            }
            return masters;
        }

    }
}