﻿using GamePackets.Server;
using GameServer.Data;
using GameServer.Networking;
using GameServer.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.PlayerCommands
{
    public class PlayerCommandSS : PlayerCommand
    {
        [Field(Position = 0)]
        public int Step;

        public override void Execute()
        {
            switch (Step)
            {
                case 0:
                    SendPacket(359, 70, new byte[] { 6, 0, 0, 0, 5, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 3, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 4, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 5, 0, 0, 0, 255, 255, 255, 255 });
                    break;
                case 1:
                    SendPacket(187, 13, new byte[] { 1, 112, 0, 6, 0, 0, 0, 0, 0, 0, 128 });
                    break;
                case 2:
                    SendPacket(187, 13, new byte[] { 6, 0, 0, 6, 0, 0, 0, 50, 0, 0, 0 });
                    break;
                case 3:
                    SendPacket(187, 13, new byte[] { 1, 171, 3, 6, 0, 0, 0, 14, 0, 0, 0 });
                    break;
                case 4:
                    SendPacket(187, 13, new byte[] { 1, 207, 3, 6, 0, 0, 0, 0, 0, 0, 128 });
                    break;
                case 5:
                    SendPacket(187, 13, new byte[] { 1, 50, 0, 6, 0, 0, 0, 32, 14, 0, 0 });
                    break;
                default:
                    break;
            }
        }

        private void SendPacket(ushort type, int length, byte[] data)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                sb.AppendLine(data[i].ToString());
            }

            var output = sb.ToString();

            var buffer = new byte[2 + (length == 0 ? 2 : 0) + data.Length];

            using var ms = new MemoryStream(buffer);
            using var bw = new BinaryWriter(ms);

            bw.Write((ushort)type);
            if (length == 0) bw.Write((ushort)buffer.Length);
            bw.Write(data);

            for (var i = 4; i < buffer.Length; i++)
                buffer[i] ^= GamePacket.EncryptionKey;

            Player.ActiveConnection.Connection.Client.Send(buffer);
        }
    }
}
