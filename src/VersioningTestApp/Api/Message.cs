namespace VersioningTestApp.Api {
    using System.Runtime.Serialization;


    [DataContract]
    public class Message {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Body { get; set; }

        public Message() {
        }

        public Message(string body, string title) {
            this.Body = body;
            this.Title = title;
        }
    }
}