﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Ray.BiliBiliTool.Agent.BiliBiliAgent.Dtos;
using Refit;

namespace Ray.BiliBiliTool.Agent.BiliBiliAgent.Interfaces
{
    /// <summary>
    /// 视频相关接口
    /// </summary>
    public interface IVideoApi : IBiliBiliApi
    {
        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        [Get("/x/web-interface/view?aid={aid}")]
        Task<BiliApiResponse<VideoDetail>> GetVideoDetail(string aid);

        /// <summary>
        /// 获取某分区下X日内排行榜
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        [Get("/x/web-interface/ranking/region?rid={rid}&day={day}")]
        Task<BiliApiResponse<List<RankingInfo>>> GetRegionRankingVideos(int rid, int day);

        /// <summary>
        /// 获取指定Up的视频
        /// </summary>
        /// <param name="upId"></param>
        /// <param name="pageSize">[1,100]验证不通过接口会报异常</param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [Get("/x/space/arc/search?mid={upId}&ps={pageSize}&tid=0&pn={pageNumber}&keyword={keyword}&order=pubdate&jsonp=jsonp")]
        Task<BiliApiResponse<SearchUpVideosResponse>> SearchVideosByUpId(long upId, int pageSize = 20, int pageNumber = 1, string keyword = "");

        /// <summary>
        /// 获取当前用户对<paramref name="aid"/>视频的投币信息
        /// </summary>
        /// <param name="aid"></param>
        /// <returns></returns>
        [Get("/x/web-interface/archive/coins?aid={aid}")]
        Task<BiliApiResponse<DonatedCoinsForVideo>> GetDonatedCoinsForVideo(string aid);

        /// <summary>
        /// 为视频投币
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="multiply"></param>
        /// <param name="select_like"></param>
        /// <param name="csrf"></param>
        /// <returns></returns>
        [Post("/x/web-interface/coin/add?aid={aid}&multiply={multiply}&select_like={select_like}&cross_domain=true&csrf={csrf}")]
        Task<BiliApiResponse> AddCoinForVideo(string aid, int multiply, int select_like, string csrf);

        /// <summary>
        /// 分享视频
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="csrf"></param>
        /// <returns></returns>
        [Post("/x/web-interface/share/add?aid={aid}&csrf={csrf}")]
        Task<BiliApiResponse> ShareVideo(string aid, string csrf);

        /// <summary>
        /// 上传视频观看进度
        /// </summary>
        /// <returns></returns>
        [Post("/x/click-interface/web/heartbeat?aid={aid}&played_time={playedTime}")]
        Task<BiliApiResponse> UploadVideoHeartbeat(string aid, int playedTime);
    }
}
