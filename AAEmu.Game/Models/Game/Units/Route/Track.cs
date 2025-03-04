﻿using System;

using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units.Movements;
using AAEmu.Game.Utils;

namespace AAEmu.Game.Models.Game.Units.Route
{

    class Track : Patrol
    {
        float distance = 1.5f;
        float MovingDistance = 0.27f;
        public override void Execute(Npc npc)
        {
            Interrupt = false;
            var move = false;
            if (npc.CurrentTarget != null)
            {

                var x = npc.Transform.World.Position.X - npc.CurrentTarget.Transform.World.Position.X;
                var y = npc.Transform.World.Position.Y - npc.CurrentTarget.Transform.World.Position.Y;
                var z = npc.Transform.World.Position.Z - npc.CurrentTarget.Transform.World.Position.Z;
                var MaxXYZ = Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z));
                float tempMovingDistance;

                // TODO: the assignment to Transform.Local here is the wrong way to do this, it will break when NPC is moving of a vehicle or slave
                if (Math.Abs(x) > distance)
                {
                    if (Math.Abs(MaxXYZ - Math.Abs(x)) > tolerance)
                    {
                        tempMovingDistance = Math.Abs(x) / (MaxXYZ / MovingDistance);
                    }
                    else
                    {
                        tempMovingDistance = MovingDistance;
                    }

                    if (x < 0)
                    {
                        npc.Transform.Local.Translate(tempMovingDistance, 0f, 0f);
                    }
                    else
                    {
                        npc.Transform.Local.Translate(-tempMovingDistance, 0f, 0f);
                    }
                    move = true;
                }
                if (Math.Abs(y) > distance)
                {
                    if (Math.Abs(MaxXYZ - Math.Abs(y)) > tolerance)
                    {
                        tempMovingDistance = Math.Abs(y) / (MaxXYZ / MovingDistance);
                    }
                    else
                    {
                        tempMovingDistance = MovingDistance;
                    }
                    if (y < 0)
                    {
                        npc.Transform.Local.Translate(0f, tempMovingDistance, 0f);
                    }
                    else
                    {
                        npc.Transform.Local.Translate(0f, -tempMovingDistance, 0f);
                    }
                    move = true;
                }
                if (Math.Abs(z) > distance)
                {
                    if (Math.Abs(MaxXYZ - Math.Abs(z)) > tolerance)
                    {
                        tempMovingDistance = Math.Abs(z) / (MaxXYZ / MovingDistance);
                    }
                    else
                    {
                        tempMovingDistance = MovingDistance;
                    }
                    if (z < 0)
                    {
                        npc.Transform.Local.Translate(0f, 0f, tempMovingDistance);
                    }
                    else
                    {
                        npc.Transform.Local.Translate(0f, 0f, -tempMovingDistance);
                    }
                    move = true;
                }

                if (Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z)) > 20)
                {
                    move = false;
                }

                //模拟unit
                //Simulated unit
                var moveType = (UnitMoveType)MoveType.GetType(MoveTypeEnum.Unit);

                //改变NPC坐标
                //Change NPC coordinates
                moveType.X = npc.Transform.Local.Position.X;
                moveType.Y = npc.Transform.Local.Position.Y;
                if (npc.TemplateId == 13677 || npc.TemplateId == 13676) // swimming
                {
                    moveType.Z = 98.5993f;
                }
                else if (npc.TemplateId == 13680) // shark
                {
                    moveType.Z = 95.5993f;
                }
                else // other
                {
                    moveType.Z = AppConfiguration.Instance.HeightMapsEnable ? WorldManager.Instance.GetHeight(npc.Transform.ZoneId, npc.Transform.World.Position.X, npc.Transform.World.Position.Y) : npc.Transform.World.Position.Z;
                }

                // looks in the direction of movement
                var angle = MathUtil.CalculateAngleFrom(npc, npc.CurrentTarget);
                var rotZ = MathUtil.ConvertDegreeToSByteDirection(angle);
                moveType.RotationX = 0;
                moveType.RotationY = 0;
                moveType.RotationZ = rotZ;

                moveType.ActorFlags = ActorMoveType.Run; // 5-walk, 4-run, 3-stand still
                moveType.DeltaMovement = new sbyte[3];
                moveType.DeltaMovement[0] = 0;
                moveType.DeltaMovement[1] = 127;
                moveType.DeltaMovement[2] = 0;
                moveType.Stance = EStance.Combat;        // COMBAT = 0x0, IDLE = 0x1
                moveType.Alertness = AiAlertness.Combat; // IDLE = 0x0, ALERT = 0x1, COMBAT = 0x2
                moveType.Time += 50; // has to change all the time for normal motion.

                if (move)
                {
                    // 广播移动状态
                    // Broadcast movement status
                    npc.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);
                    LoopDelay = 500;
                    Repeat(npc);
                }
                else
                {
                    // 如果小于差距则停止移动准备攻击
                    // Stop moving to prepare for attack if it is smaller than the gap
                    if (Math.Max(Math.Max(Math.Abs(x), Math.Abs(y)), Math.Abs(z)) <= distance)
                    {
                        var combat = new Combat();
                        combat.LastPatrol = LastPatrol;
                        combat.LoopDelay = 2900;
                        combat.Pause(npc);
                        LastPatrol = combat;
                    }
                    else
                    {
                        npc.BroadcastPacket(new SCCombatClearedPacket(npc.CurrentTarget.ObjId), true);
                        npc.BroadcastPacket(new SCCombatClearedPacket(npc.ObjId), true);
                        npc.CurrentTarget = null;
                        //npc.StartRegen();
                        npc.BroadcastPacket(new SCTargetChangedPacket(npc.ObjId, 0), true);
                    }
                    // 距离超过指定长度 放弃追踪 停止移动
                    // Distance exceeds the specified length Abandon Tracking Stop moving
                    moveType.DeltaMovement[1] = 0;
                    npc.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);
                    Stop(npc);
                }
            }

            if (LastPatrol == null)
            {
                // 创建直线巡航回归上次巡航暂停点
                // Create a straight cruise to return to the last cruise pause
                var line = new Line();
                // 不可中断，不受外力及攻击影响 类似于处于脱战状态
                // Uninterruptible, unaffected by external forces and attacks Similar to being in an off-war situation
                line.Interrupt = true;
                line.Loop = false;
                line.Abandon = false;
                line.Pause(npc);
                LastPatrol = line;
            }
        }
    }
}
