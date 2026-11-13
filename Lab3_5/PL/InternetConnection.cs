namespace Lab3_5.PL;

public static class InternetConnection
{
    public static bool IsConnected()
    {
        using var httpClient = new HttpClient();
        using var response = httpClient.Send(new HttpRequestMessage(HttpMethod.Head, "https://8.8.8.8"));
        return response.IsSuccessStatusCode;
    }
}
