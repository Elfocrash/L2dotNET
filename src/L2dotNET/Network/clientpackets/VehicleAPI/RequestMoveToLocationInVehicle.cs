﻿using System;
using L2dotNET.Logging.Abstraction;
using L2dotNET.Models.Player;
using L2dotNET.Models.Vehicles;
using L2dotNET.Network.serverpackets;

namespace L2dotNET.Network.clientpackets.VehicleAPI
{
    class RequestMoveToLocationInVehicle : PacketBase
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly GameClient _client;
        private readonly int _boatId;
        private readonly int _dx;
        private readonly int _dy;
        private readonly int _dz;
        private readonly int _x;
        private readonly int _y;
        private readonly int _z;

        public RequestMoveToLocationInVehicle(IServiceProvider serviceProvider, Packet packet, GameClient client) : base(serviceProvider)
        {
            _client = client;
            _boatId = packet.ReadInt(); //objectId of boat
            _dx = packet.ReadInt();
            _dy = packet.ReadInt();
            _dz = packet.ReadInt();
            _x = packet.ReadInt();
            _y = packet.ReadInt();
            _z = packet.ReadInt();
        }

        public override void RunImpl()
        {
            L2Player player = _client.CurrentPlayer;

            //You do not possess the correct ticket to board the boat.
            
            L2Boat boat = null;
            if (player.Boat != null)
            {
                if (player.Boat.ObjId == _boatId)
                    boat = player.Boat;
                else
                {
                    player.SendActionFailed();
                    return;
                }
            }
            else
            {
                if (player.KnownObjects.ContainsKey(_boatId))
                    boat = (L2Boat)player.KnownObjects[_boatId];
            }

            if (boat == null)
            {
                Log.Error($"User requested null boat {_boatId}");
                player.SendActionFailed();
                return;
            }

            if (player.Boat == null)
                player.Boat = boat;

            player.BoatX = _dx;
            player.BoatY = _dy;
            player.BoatZ = _dz;
            player.BroadcastPacket(new MoveToLocationInVehicle(player, _x, _y, _z));
        }
    }
}