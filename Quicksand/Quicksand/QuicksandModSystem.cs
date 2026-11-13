using System;
using Quicksand.network;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace Quicksand;

public class QuicksandModSystem : ModSystem
{
    public static QuicksandConfig Config { get; private set; } = new();
    
    private const string ConfigFile = "quicksand.json";
    private ICoreAPI api;
    private ICoreClientAPI capi;
    private ICoreServerAPI sapi;
    private bool clientBlind = false;
    private IServerNetworkChannel serverChannel;
    private IClientNetworkChannel clientChannel;
    public override void Start(ICoreAPI api)
    {
        this.api = api;
        base.Start(api);
        api.RegisterBlockClass("BlockQuicksand", typeof(BlockQuicksand));
        BlockQuicksand.staminaEnabled = api.ModLoader.IsModEnabled("vigor");
        api.Network
            .RegisterChannel("quicksand")
            .RegisterMessageType<ConfigMessage>()
            .RegisterMessageType<ConfigRequest>();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
        capi = api;
        clientChannel = capi.Network
            .GetChannel("quicksand")
            .SetMessageHandler<ConfigMessage>(OnConfigReceived);
    }

    private void OnConfigReceived(ConfigMessage configMessage)//Client side
    {
        QuicksandConfig serverConfig = new QuicksandConfig();
        
        serverConfig.stillSinkSpeed = configMessage.stillSinkSpeed;
        serverConfig.maxSinkSpeed = configMessage.maxSinkSpeed;
        serverConfig.climbSpeed = configMessage.climbSpeed;
        serverConfig.maxHorizontalSpeed = configMessage.maxHorizontalSpeed;
        serverConfig.friction = configMessage.friction;
        serverConfig.struggleDrain = configMessage.struggleDrain;
        serverConfig.blindEnabled = configMessage.blindEnabled;
        
        Config = serverConfig;
    }
    public override void StartServerSide(ICoreServerAPI api)
    {
        sapi = api;
        Config = LoadConfig(api);
        
        serverChannel = sapi.Network
            .GetChannel("quicksand")
            .SetMessageHandler<ConfigRequest>(OnConfigRequest);
        sapi.Event.PlayerJoin += OnPlayerJoin;
    }

    private void OnPlayerJoin(IPlayer player)
    {
        if (api.Side == EnumAppSide.Server)
        {
            serverChannel.SendPacket(new ConfigMessage() 
            {stillSinkSpeed =  Config.stillSinkSpeed,
                maxSinkSpeed = Config.maxSinkSpeed,
                climbSpeed = Config.climbSpeed,
                maxHorizontalSpeed = Config.maxHorizontalSpeed,
                friction = Config.friction,
                struggleDrain = Config.struggleDrain,
                blindEnabled = Config.blindEnabled }, (IServerPlayer) player);
        }
    }

    private void OnConfigRequest(IServerPlayer fromPlayer, ConfigRequest packet)
    {
        serverChannel.SendPacket(new ConfigMessage() 
            {stillSinkSpeed =  Config.stillSinkSpeed,
                maxSinkSpeed = Config.maxSinkSpeed,
                climbSpeed = Config.climbSpeed,
                maxHorizontalSpeed = Config.maxHorizontalSpeed,
                friction = Config.friction,
                struggleDrain = Config.struggleDrain,
                blindEnabled = Config.blindEnabled }, fromPlayer);
    }
    
    


    private QuicksandConfig LoadConfig(ICoreAPI api)
    {
        try
        {
            var config = api.LoadModConfig<QuicksandConfig>(ConfigFile) ?? new QuicksandConfig();
            api.StoreModConfig(config, ConfigFile);
            return config;
        }
        catch (Exception e)
        {
            Mod.Logger.Warning("Failed to load {0}, using defaults: {1}", ConfigFile, e.Message);
            return new QuicksandConfig();
        }
    }
}