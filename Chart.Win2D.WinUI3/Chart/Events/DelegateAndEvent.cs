using System;
using System.Threading;

namespace ChartBase.Chart.Events;

public class DelegateAndEvent
{
    static void main(string[] args)
    {
        var video = new Video() { Title="Video 1"};
        var videoEncoder = new VideoEncoder();      // publisher
        var mailService = new MailService();        // subscriber
        var messageService = new MessageService();  // subscriber

        videoEncoder.VideoEncoded += mailService.OnVideoEncoded;
        videoEncoder.VideoEncoded += messageService.OnVideoEncoded;

        videoEncoder.Encode(video);
    }
    
}

public class Video
{
    public string Title { get; set; }  
}

public class VideoEventArgs : EventArgs
{
    public Video Video { get; set; }    
}



public class VideoEncoder
{
    // 1. Define a delegate
    // 2. Define an event based on that delegate
    // 3. Raise the Event

    public delegate void VideoEncodedEventHandler(Object source, VideoEventArgs args);
    public event VideoEncodedEventHandler VideoEncoded;

    //
    // There are following built-in delegates we can use so we don't need create delegate for Event by ourselves
    //
    // . EventHandler
    // . EventHandler<TEventArgs>
    //
    // For example:
    // public event EventHandler<VideoEventArg> VideoEncoded;
    //

    public void Encode(Video video)
    {
        Console.WriteLine("Cncoding Video ...");
        Thread.Sleep(1000);

        OnVideoEncoded(video);
    }

    private void OnVideoEncoded(Video video)
    {
        VideoEncoded?.Invoke(this, new VideoEventArgs() { Video = video });
    }
}

public class MailService
{
    public void OnVideoEncoded(Object source, VideoEventArgs e)
    {
        Console.WriteLine("Mail Service: Sending a text message... "+e.Video.Title);
    }
}

public class MessageService
{
    public void OnVideoEncoded(object source, VideoEventArgs e)
    {
        Console.WriteLine("MessageService: Sending a text message: "+ e.Video.Title);
    }
}

