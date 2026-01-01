namespace SampleWebApi.Model.DbContexts
{
    public class GameEvent
    {
        public int Id { get; set; } // 이벤트 고유 ID

        public string EventType { get; set; } // 이벤트 타입

        public string Payload { get; set; } = string.Empty;// 이벤트 데이터

        public int UserId { get; set; } = -1;

        public string EventVersion { get; set; } //이벤트 처리기 버전관리용

        public string Description { get; set; } = string.Empty;//로그
    }
}
