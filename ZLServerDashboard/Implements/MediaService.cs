using AutoMapper;
using Microsoft.EntityFrameworkCore;
using STRealVideo.Lib;
using STRealVideo.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Commons;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Interface;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Implements
{
    public class MediaService : IMediaService
    {
        private readonly MediaPlatContext dbContext;
        private IMapper mapper;
        public MediaService(MediaPlatContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        #region  Domain
        public bool AddDomain(DomainDto domain, UserDto opera)
        {
            domain.Id = Tools.NewID;
            domain.CreateBy = opera.Id;
            domain.CreateTs = DateTime.Now;
            domain.UpdateBy = opera.Id;
            domain.UpdateTs = domain.CreateTs;

            var dbModle = mapper.Map<TbDomain>(domain);
            dbContext.TbDomain.Add(dbModle);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                domain.CreateByNavigation = opera;
                domain.UpdateByNavigation = opera;
                RedisHelper.Instance.SetHash<DomainDto>(domain.Id.ToString(), domain);
            }
            return flag;
        }

        public bool DeleteDomain(long[] domainIDs, UserDto opera)
        {
            var dbModels = dbContext.TbDomain.Where(p => domainIDs.Contains(p.Id)).ToList();
            foreach (var item in dbModels)
            {
                item.Status = (int)DomainState.Deleted;
                RedisHelper.Instance.DeleteHash<DomainDto>(item.Id.ToString());
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                foreach (var item in dbModels)
                {
                    RedisHelper.Instance.DeleteHash<DomainDto>(item.Id.ToString());
                }
            }
            return flag;
        }

        public bool EditDomain(DomainDto domain, UserDto opera)
        {
            var dbModle = dbContext.TbDomain.Include(p => p.CreateByNavigation).Where(predicate => predicate.Id == domain.Id).FirstOrDefault();
            if (dbModle == null)
                return false;
            dbModle.DomainName = domain.DomainName;
            dbModle.Remark = domain.Remark;
            dbModle.Status = (int)domain.Status;
            dbModle.UpdateBy = opera.Id;
            dbModle.UpdateTs = DateTime.Now;
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                var dto = mapper.Map<DomainDto>(dbModle);
                dto.UpdateByNavigation = opera;
                RedisHelper.Instance.SetHash<DomainDto>(domain.Id.ToString(), domain);
            }
            return flag;
        }

        public DomainDto GetDomain(long id)
        {
            var domain = RedisHelper.Instance.GetHash<DomainDto>(id);
            if (domain == null)
            {
                domain = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Id == id).Select(p => mapper.Map<DomainDto>(p)).FirstOrDefault();
                if (domain != null)
                {
                    RedisHelper.Instance.SetHash<DomainDto>(domain.Id.ToString(), domain);
                }
            }
            return domain;
        }

        public TableQueryModel<DomainDto> GetDomainList(QueryModel queryModel)
        {
            TableQueryModel<DomainDto> result = new TableQueryModel<DomainDto>();
            // var domainList = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).Select(p => mapper.Map<DomainDto>(p)).AsQueryable();
            // if (domainList == null || domainList.Count == 0)
            // {
            //     domainList = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).Select(p => mapper.Map<DomainDto>(p)).ToList();
            //     foreach (var item in domainList)
            //     {
            //         RedisHelper.Instance.SetHash<DomainDto>(item.Id.ToString(), item);
            //     }
            // }

            var query = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).AsQueryable();
            if (!String.IsNullOrWhiteSpace(queryModel.keyword))
            {
                query = query.Where(p =>
                 p.DomainName.Contains(queryModel.keyword) &&
                 p.Remark.Contains(queryModel.keyword)
                ).AsQueryable();
            }
            result.count = query.Count();
            query = query.Skip((queryModel.page - 1) * queryModel.limit);
            if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
            {

                query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());

            }
            query = query.Take(queryModel.limit);
            result.code = 0;
            result.data = query.Select(p => mapper.Map<DomainDto>(p)).ToList();
            return result;
        }

        public List<DomainDto> GetAllNoramlDomain()
        {
            List<DomainDto> result = RedisHelper.Instance.GetHashAllObj<DomainDto>().ToList();
            if (result == null || result.Count == 0)
            {
                result = dbContext.TbDomain.Where(p => p.Status != (int)DomainState.Deleted).Select(p => mapper.Map<DomainDto>(p)).ToList();
                foreach (var item in result)
                {
                    RedisHelper.Instance.SetHash<DomainDto>(item.Id.ToString(), item);
                }
            }
            return result.Where(p => p.Status == DomainState.Normal).ToList(); ;
        }

        #endregion


        #region  Application
        public bool AddApplication(ApplicationDto domain, UserDto opera)
        {
            domain.Id = Tools.NewID;
            domain.CreateBy = opera.Id;
            domain.CreateTs = DateTime.Now;
            domain.UpdateBy = opera.Id;
            domain.UpdateTs = domain.CreateTs;

            var dbModle = mapper.Map<TbApplication>(domain);
            dbContext.TbApplication.Add(dbModle);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                domain.CreateByNavigation = opera;
                domain.UpdateByNavigation = opera;
                RedisHelper.Instance.SetHash<ApplicationDto>(domain.Id.ToString(), domain);
            }
            return flag;
        }

        public bool DeleteApplication(long[] domainIDs, UserDto opera)
        {
            var dbModels = dbContext.TbApplication.Where(p => domainIDs.Contains(p.Id)).ToList();
            foreach (var item in dbModels)
            {
                item.Status = (int)DomainState.Deleted;
                RedisHelper.Instance.DeleteHash<ApplicationDto>(item.Id.ToString());
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                foreach (var item in dbModels)
                {
                    RedisHelper.Instance.DeleteHash<ApplicationDto>(item.Id.ToString());
                }
            }
            return flag;
        }

        public bool EditApplication(ApplicationDto domain, UserDto opera)
        {
            var dbModle = dbContext.TbApplication.Include(p => p.CreateByNavigation).Where(predicate => predicate.Id == domain.Id).FirstOrDefault();
            if (dbModle == null)
                return false;
            dbModle.App = domain.App;
            dbModle.Remark = domain.Remark;
            dbModle.Status = (int)domain.Status;
            dbModle.UpdateBy = opera.Id;
            dbModle.UpdateTs = DateTime.Now;
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                var dto = mapper.Map<ApplicationDto>(dbModle);
                dto.UpdateByNavigation = opera;
                RedisHelper.Instance.SetHash<ApplicationDto>(domain.Id.ToString(), domain);
            }
            return flag;
        }

        public ApplicationDto GetApplication(long id)
        {
            var domain = RedisHelper.Instance.GetHash<ApplicationDto>(id);
            if (domain == null)
            {
                domain = dbContext.TbApplication.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Id == id).Select(p => mapper.Map<ApplicationDto>(p)).FirstOrDefault();
                if (domain != null)
                {
                    RedisHelper.Instance.SetHash<ApplicationDto>(domain.Id.ToString(), domain);
                }
            }
            return domain;
        }

        public TableQueryModel<ApplicationDto> GetApplicationList(QueryModel queryModel)
        {
            TableQueryModel<ApplicationDto> result = new TableQueryModel<ApplicationDto>();


            var query = dbContext.TbApplication.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).AsQueryable();
            if (!String.IsNullOrWhiteSpace(queryModel.keyword))
            {
                query = query.Where(p =>
                 p.App.Contains(queryModel.keyword) &&
                 p.Remark.Contains(queryModel.keyword)
                ).AsQueryable();
            }
            result.count = query.Count();
            query = query.Skip((queryModel.page - 1) * queryModel.limit);
            if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
            {

                query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());

            }
            query = query.Take(queryModel.limit);
            result.code = 0;
            result.data = query.Select(p => mapper.Map<ApplicationDto>(p)).ToList();
            return result;
        }


        public List<ApplicationDto> GetAllNoramlApplication()
        {
            List<ApplicationDto> result = RedisHelper.Instance.GetHashAllObj<ApplicationDto>().ToList();
            if (result == null || result.Count == 0)
            {
                result = dbContext.TbApplication.Where(p => p.Status != (int)ApplicationState.Deleted).Select(p => mapper.Map<ApplicationDto>(p)).ToList();
                foreach (var item in result)
                {
                    RedisHelper.Instance.SetHash<ApplicationDto>(item.Id.ToString(), item);
                }
            }
            return result.Where(p => p.Status == ApplicationState.Normal).ToList();
        }

        #endregion



        #region  StreamProxy
        public BaseModel<String> AddStreamProxy(StreamProxyDto streamProxyDto, UserDto opera)
        {
            BaseModel<String> result = new BaseModel<string>();
            var domain = GetDomain(streamProxyDto.DomainId);
            if (domain == null)
                return result.Failed("找不到该域名!");
            var app = GetApplication(streamProxyDto.AppId);
            if (app == null)
                return result.Failed("找不到该应用!");

            if (dbContext.TbStreamProxy.Where(p => p.DomainId == streamProxyDto.Id
             && p.AppId == streamProxyDto.AppId
             && p.StreamName == streamProxyDto.StreamName
             && p.State != (int)StreamProxyState.Deleted
            ).Any())
            {
                return result.Failed("存在相同流名称!");
            }

            streamProxyDto.Id = Tools.NewID;
            streamProxyDto.CreateBy = opera.Id;
            streamProxyDto.CreateTs = DateTime.Now;
            streamProxyDto.UpdateBy = opera.Id;
            streamProxyDto.UpdateTs = streamProxyDto.CreateTs;

            var dbModle = mapper.Map<TbStreamProxy>(streamProxyDto);
            dbContext.TbStreamProxy.Add(dbModle);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                streamProxyDto.CreateByNavigation = opera;
                streamProxyDto.UpdateByNavigation = opera;
                RedisHelper.Instance.SetHash<StreamProxyDto>(streamProxyDto.Id.ToString(), streamProxyDto);
                result.Success("添加成功!");
                if (app.Status == ApplicationState.Normal && domain.Status == DomainState.Normal && streamProxyDto.State == StreamProxyState.Normal)
                {
                    //开始拉流逻辑
                    AddStreamProxy(streamProxyDto, domain, app);
                }

            }
            return result;
        }

        public bool DeleteStreamProxy(long[] streamIDs, UserDto opera)
        {
            var dbModels = dbContext.TbStreamProxy.Where(p => streamIDs.Contains(p.Id)).ToList();
            foreach (var item in dbModels)
            {
                item.State = (int)StreamProxyState.Deleted;
                RedisHelper.Instance.DeleteHash<StreamProxyDto>(item.Id.ToString());
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                foreach (var item in dbModels)
                {
                    //执行断流逻辑
                    RedisHelper.Instance.DeleteHash<StreamProxyDto>(item.Id.ToString());
                    StopStreamProxy(mapper.Map<StreamProxyDto>(item));
                }
            }
            return flag;
        }

        public BaseModel<String> EditStreamProxy(StreamProxyDto streamProxyDto, UserDto opera = null)
        {
            BaseModel<String> result = new BaseModel<string>();
            var dbModle = dbContext.TbStreamProxy.Include(p => p.CreateByNavigation).Where(predicate => predicate.Id == streamProxyDto.Id).FirstOrDefault();
            if (dbModle == null)
                return result.Failed("找不到数据!");

            var domain = GetDomain(streamProxyDto.DomainId);
            if (domain == null)
                return result.Failed("找不到该域名!");
            var app = GetApplication(streamProxyDto.AppId);
            if (app == null)
                return result.Failed("找不到该应用!");

            if (dbContext.TbStreamProxy.Where(p => p.DomainId == streamProxyDto.Id
             && p.Id != streamProxyDto.Id
             && p.AppId == streamProxyDto.AppId
             && p.StreamName == streamProxyDto.StreamName
             && p.State != (int)StreamProxyState.Deleted
            ).Any())
            {
                return result.Failed("存在相同流名称!");
            }

            StopStreamProxy(mapper.Map<StreamProxyDto>(dbModle), domain, app);

            if (opera != null)
            {
                dbModle.UpdateBy = opera.Id;
                dbModle.UpdateTs = DateTime.Now;
            }

            dbModle.AppId = streamProxyDto.AppId;
            dbModle.DomainId = streamProxyDto.DomainId;
            dbModle.Name = streamProxyDto.Name;
            dbModle.Remark = streamProxyDto.Remark;
            dbModle.StreamName = streamProxyDto.StreamName;
            dbModle.SourceUrl = streamProxyDto.SourceUrl;
            dbModle.EnableHls = streamProxyDto.EnableHls;
            dbModle.EnableMp4 = streamProxyDto.EnableMp4;
            dbModle.EnableRtmp = streamProxyDto.EnableRtmp;
            dbModle.EnableRtsp = streamProxyDto.EnableRtsp;
            dbModle.RtpType = (int)streamProxyDto.RtpType;
            dbModle.State = (int)streamProxyDto.State;

            var flag = dbContext.SaveChanges() > 0 ? true : false;
            var dto = mapper.Map<StreamProxyDto>(dbModle);
            if (flag)
            {
                dto.UpdateByNavigation = opera;
                
                result.Success("保存成功!");
                if (app.Status == ApplicationState.Normal && domain.Status == DomainState.Normal && streamProxyDto.State == StreamProxyState.Normal)
                {
                    //开始拉流逻辑
                    AddStreamProxy(dto, domain, app);
                }
                else
                {
                    //执行断流逻辑
                    StopStreamProxy(mapper.Map<StreamProxyDto>(dbModle), domain, app);
                }
            }
            RedisHelper.Instance.SetHash<StreamProxyDto>(dto.Id.ToString(), dto);
            return result;
        }

        public StreamProxyDto GetStreamProxy(long id)
        {
            var domain = RedisHelper.Instance.GetHash<StreamProxyDto>(id);
            domain=null;
            if (domain == null)
            {
                domain = dbContext.TbStreamProxy.
                Include(p => p.CreateByNavigation).
                Include(p => p.UpdateByNavigation).
                Where(predicate => predicate.Id == id).
                Select(p => mapper.Map<StreamProxyDto>(p)).
                FirstOrDefault();
                if (domain != null)
                {
                    RedisHelper.Instance.SetHash<StreamProxyDto>(domain.Id.ToString(), domain);
                }
            }
            return domain;
        }

        public TableQueryModel<StreamProxyDto> GetStreamProxyList(QueryModel queryModel)
        {
            TableQueryModel<StreamProxyDto> result = new TableQueryModel<StreamProxyDto>();
            // var domainList = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).Select(p => mapper.Map<DomainDto>(p)).AsQueryable();
            // if (domainList == null || domainList.Count == 0)
            // {
            //     domainList = dbContext.TbDomain.Include(p => p.CreateByNavigation).Include(p => p.UpdateByNavigation).Where(predicate => predicate.Status != (int)DomainState.Deleted).Select(p => mapper.Map<DomainDto>(p)).ToList();
            //     foreach (var item in domainList)
            //     {
            //         RedisHelper.Instance.SetHash<DomainDto>(item.Id.ToString(), item);
            //     }
            // }

            var query = dbContext.TbStreamProxy
            .Include(p => p.CreateByNavigation)
            .Include(p => p.UpdateByNavigation)
            .Include(p => p.App)
            .Include(p => p.Domain)
            .Where(predicate => predicate.State != (int)StreamProxyState.Deleted).AsQueryable();
            if (!String.IsNullOrWhiteSpace(queryModel.keyword))
            {
                query = query.Where(p =>
                 p.Name.Contains(queryModel.keyword) ||
                 p.Remark.Contains(queryModel.keyword) ||
                 p.StreamName.Contains(queryModel.keyword) ||
                 p.SourceUrl.Contains(queryModel.keyword)
                ).AsQueryable();
            }
            result.count = query.Count();
            query = query.Skip((queryModel.page - 1) * queryModel.limit);
            if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
            {
                query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
            }
            query = query.Take(queryModel.limit);
            result.code = 0;
            result.data = query.Select(p => mapper.Map<StreamProxyDto>(p)).ToList();
            return result;
        }

        #endregion



        #region  媒体服务器操作
        private CloseStreamsResponse StopStreamProxy(StreamProxyDto dto, DomainDto domain = null, ApplicationDto application = null)
        {
            return null;

            ConsoleHelper.Failed("开始断流:" + dto.StreamName);
            if (domain == null)
                domain = GetDomain(dto.DomainId);
            if (application == null)
                application = GetApplication(dto.AppId);
            return Tools.ZLClient.closeStreams(null, domain.DomainName, application.App, dto.StreamName, true);
        }

        public ZLResponse<BodyKey> AddStreamProxy(StreamProxyDto dto, DomainDto domain = null, ApplicationDto application = null)
        {

            if (domain == null)
                domain = GetDomain(dto.DomainId);
            if (application == null)
                application = GetApplication(dto.AppId);
            ConsoleHelper.Warning("开始拉流:" + dto.Name + ",域名[" + domain.DomainName + "],应用:[" + application.App + "],流[" + dto.StreamName + "]");

            var resp = Tools.ZLClient.addStreamProxy(
                domain.DomainName,
                application.App,
                dto.StreamName,
                dto.SourceUrl,
                dto.EnableRtsp,
                dto.EnableRtmp,
                dto.EnableHls,
                dto.EnableMp4,
                (STRealVideo.Lib.RTPPullType)(int)dto.RtpType
                );

            if (resp.code != 0 || resp.data == null || String.IsNullOrWhiteSpace(resp.data.key))
            {
                dto.State = StreamProxyState.Forbid;
                //拉流失败
                EditStreamProxy(dto);
                ConsoleHelper.Failed("拉流失败:" + dto.Name + ",域名[" + domain.DomainName + "],应用:[" + application.App + "],流[" + dto.StreamName + "]");
            }
            else
            {
                ConsoleHelper.Success("拉流成功:" + dto.Name + ",域名[" + domain.DomainName + "],应用:[" + application.App + "],流[" + dto.StreamName + "]");
            }

            return resp;
        }

        public List<MediaStream> GetStreamInfos(string domain, string app)
        {
            return new List<MediaStream>();
            var data = Tools.ZLClient.getMediaList(null, domain, app).data;
            if (data == null)
                data = new List<MediaStream>();
            return data;
        }

        public MediaStream GetStreamInfo(string domain, string app, string steamName)
        {
            return GetStreamInfos(domain, app).Where(p => p.stream == steamName).FirstOrDefault();
        }

        #endregion
    }
}
