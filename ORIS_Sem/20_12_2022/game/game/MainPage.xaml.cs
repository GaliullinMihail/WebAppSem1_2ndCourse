using XProtocol.Serializator;
using XProtocol;
using game.Client;

namespace game;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        if (XClient.IsRegistered)
        {
            XClient.QueuePacketSend(
                    XPacketConverter.Serialize(
                        XPacketType.Register,
                        new XPacketRegister
                        {
                        }).ToPacket());
        }
        await Shell.Current.GoToAsync("GamePage");
    }
}

