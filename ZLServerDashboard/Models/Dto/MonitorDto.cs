using System;
using static ZLServerDashboard.Models.Enums;

namespace ZLServerDashboard.Models.Dto
{
    public class MonitorDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public LinkType LinkType { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTs { get; set; }
        public MonitorStatus Status { get; set; }
        public long Zid { get; set; }
        public long QuestionCount { get; set; }
        public  UserDto CreateByNavigation { get; set; }

        public String CreateByName {get=>CreateByNavigation?.Name;}
        public long FollowerCount{get;set;}
        public long EssenceQuestionCount{get;set;}
        public long ViewCount{get;set;}
        public long AnswerCount{get;set;}
        public long CommentCount{get;set;}
        public long CollapsedAnswerCount{get;set;}

        

        
    }
}