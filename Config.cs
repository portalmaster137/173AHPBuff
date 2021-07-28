using System.Collections.Generic;
using Exiled.API.Features;
using System;
using Exiled.API.Interfaces;
using Exiled.Events.Handlers;
using Exiled.Loader;
using UnityEngine;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using MEC;

namespace _173AHPBuff
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int MaxAHP { get; set; } = 1000;
        public int DelayTime { get; set; } = 10;
       
    }
}