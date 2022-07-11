
namespace MinimalApi;
public class TwitterService : ITwitterService
{
    private readonly IConfiguration _config;
    public TwitterClient TwitterClient { get; set; }
    public TwitterService(IConfiguration config)
    {
        _config = config;
        TwitterClient = BuildClient().Result;
    }

    public Task<TwitterClient> BuildClient(
        string CONSUMER_KEY,
        string CONSUMER_SECRET,
        string ACCESS_TOKEN,
        string ACCESS_TOKEN_SECRET)
    {
        return Task.FromResult(new TwitterClient(
            CONSUMER_KEY,
            CONSUMER_SECRET,
            ACCESS_TOKEN,
            ACCESS_TOKEN_SECRET));
    }

    public Task<TwitterClient> BuildClient() =>
        Task.FromResult(new TwitterClient(_config["CONSUMER_KEY"],
                                                 _config["CONSUMER_SECRET"],
                                                 _config["ACCESS_TOKEN"],
                                                 _config["ACCESS_TOKEN_SECRET"]));


    public async Task<TweetV2> GetTweetById(long id) =>
        (await TwitterClient.TweetsV2.GetTweetAsync(id)).Tweet;

}
