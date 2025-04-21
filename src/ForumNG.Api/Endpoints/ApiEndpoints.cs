namespace ForumNG.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "/api";

    public static class Posts
    {
        private const string Base = $"{ApiBase}/posts";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAllByTopicId = $"{ApiBase}/topics/{{topicId:guid}}/posts";
    }

    public static class Topics
    {
        private const string Base = $"{ApiBase}/topics";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = Base;
    }

    public static class Users
    {
        private const string Base = $"{ApiBase}/users";
        public const string Get = $"{Base}/{{id:guid}}";
    }
}
