﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WPFClient
{
    public enum CommandType
    {
        DrawOpStart = 1,
        DrawOpMove,
        DrawOpEnd,
        DrawOpUndo,
        DrawOpPktId,
        DrawOpClear,
        DrawOpClearCb,
        DrawOpDrawPanelSize,
        DrawOpSrollDrawPanel,
        DrawOpBeginTeach,
    }

    //接收命令
    public class PaintCommand
    {
        public CommandType Type { get; set; }
        public PointF Coord;

        public static PaintCommand Create(string cmdStr, int width,int height)
        {
            var c = cmdStr.Split(new char[] { ':', ',' });
            if (c.Length >= 3)
            {
                PaintCommand cmd = new PaintCommand();
                cmd.Type = (CommandType)int.Parse(c[0]);
                cmd.Coord.X = float.Parse(c[1]) * height;
                cmd.Coord.Y = float.Parse(c[2]) * height;
                return cmd;
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("Type:{0} X:{1} Y:{2}", Type, Coord.X, Coord.Y);
        }

    }
    public class PaintingRecord
    {
        private readonly List<PaintCommand> _paintingCommands;
        private int _index;
        public double width;
        public double height;

        public string rgb;
        public string lineSize;
        public PaintingRecord()
        {
            _paintingCommands = new List<PaintCommand>();
            width = height = 1;
        }

        public void Add(CommandType type,float x, float y)
        {
            PaintCommand cmd = new PaintCommand();
            cmd.Type = type;
            cmd.Coord.X = x;
            cmd.Coord.Y = y;
            _paintingCommands.Add(cmd);
        }

        public void Add(PaintCommand cmd)
        {
            _paintingCommands.Add(cmd);
        }

        public int Count
        {
            get { return _paintingCommands.Count; }
        }

        public PaintCommand this[int index]
        {
            get
            {
                if (index < 0 || index >= _paintingCommands.Count)
                    throw new IndexOutOfRangeException();
                return _paintingCommands[index];
            }
        }

        public string CreateCommand()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = _index; i < _paintingCommands.Count; i++)
            {
                var cmd = _paintingCommands[i];
                var s = string.Format("{0}:{1},{2},{3},{4};", (int)cmd.Type, cmd.Coord.X / width, cmd.Coord.Y / height,rgb,lineSize);
                //var s = string.Format("{0}:{1},{2};", (int)cmd.Type, cmd.Coord.X / width, cmd.Coord.Y / height);
                stringBuilder.Append(s);
            }
            _index = _paintingCommands.Count;
            return stringBuilder.ToString();
        }

        public void Revert()
        {
            int index = _paintingCommands.Count - 1;
            while (index >= 0 && _paintingCommands[index].Type != CommandType.DrawOpStart)
                index--;
            if (index >= 0)
                _paintingCommands.RemoveRange(index, _paintingCommands.Count - index);
        }

        public void Clear()
        {
            _index = 0;
            _paintingCommands.Clear();
        }
    }
}
