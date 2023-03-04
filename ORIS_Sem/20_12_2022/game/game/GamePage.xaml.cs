using XProtocol.Serializator;
using XProtocol;
using game.Client;

namespace game;

public partial class GamePage: ContentPage
{
	public GamePage()
	{
		InitializeComponent();
        Task.Run((Action)SendCursorPackets);
    }

    private void PauseBtn_Clicked(object sender, EventArgs e)	//46 ïèêñåëåé ñëåâà 46 ïèêñåëåé ñïðàâà 123 ïèêñåëÿ ñâåðõó
    {

    }

    private void SendCursorPackets()
    {
        while (true)
        {
            if (XClient.GameIsPaused)
            {
                Thread.Sleep(100);
                continue;
            }
            XClient.QueuePacketSend(
                XPacketConverter.Serialize(
                    XPacketType.SendCursorPosition, new XPacketCursor
                    {
                        Id = XClient.Id,
                        X = 1,  //TODO: change to cursor pos
                        Y = 2   //TODO: change to cursor pos 
                    }).ToPacket());

        }
    }
}