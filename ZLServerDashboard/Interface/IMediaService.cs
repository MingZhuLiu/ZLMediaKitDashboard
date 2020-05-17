using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using STRealVideo.Lib;
using STRealVideo.Lib.Models;
using ZLServerDashboard.Models;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Interface
{
    public interface IMediaService
    {
        #region  Domain
        bool AddDomain(DomainDto domain, UserDto opera);

        bool EditDomain(DomainDto domain, UserDto opera);

        bool DeleteDomain(long[] domainIDs, UserDto opera);

        TableQueryModel<DomainDto> GetDomainList(QueryModel queryModel);

        DomainDto GetDomain(long id);

        #endregion



        #region  Application
        bool AddApplication(ApplicationDto domain, UserDto opera);

        bool EditApplication(ApplicationDto domain, UserDto opera);

        bool DeleteApplication(long[] appIDs, UserDto opera);

        TableQueryModel<ApplicationDto> GetApplicationList(QueryModel queryModel);

        ApplicationDto GetApplication(long id);

        #endregion


        #region  StreamProxy
        BaseModel<String> AddStreamProxy(StreamProxyDto streamProxyDto, UserDto opera);

        BaseModel<String> EditStreamProxy(StreamProxyDto streamProxyDto, UserDto opera);

        bool DeleteStreamProxy(long[] streamIDs, UserDto opera);

        TableQueryModel<StreamProxyDto> GetStreamProxyList(QueryModel queryModel);

        StreamProxyDto GetStreamProxy(long id);


        List<DomainDto> GetAllNoramlDomain();

        List<ApplicationDto> GetAllNoramlApplication();

        List<MediaStream> GetStreamInfos(string domain,string app);
        MediaStream GetStreamInfo(string domain,string app,string steamName);

        ZLResponse<BodyKey> AddStreamProxy(StreamProxyDto dto, DomainDto domain = null, ApplicationDto application = null);

        #endregion

    }
}
