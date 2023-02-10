// Copyright (c) coherence ApS.
// For all coherence generated code, the coherence SDK license terms apply. See the license file in the coherence Package root folder for more information.

// <auto-generated>
// Generated file. DO NOT EDIT!
// </auto-generated>
namespace Coherence.Generated
{
	using Coherence.ProtocolDef;
	using Coherence.Serializer;
	using Coherence.SimulationFrame;
	using Coherence.Entity;
	using Coherence.Utils;
	using Coherence.Brook;
	using Coherence.Toolkit;
	using UnityEngine;

	public struct WorldPosition : ICoherenceComponentData
	{
		public Vector3 value;

		public override string ToString()
		{
			return $"WorldPosition(value: {value})";
		}

		public uint GetComponentType() => Definition.InternalWorldPosition;

		public const int order = 0;

		public int GetComponentOrder() => order;

		public AbsoluteSimulationFrame Frame;
	

		public void SetSimulationFrame(AbsoluteSimulationFrame frame)
		{
			Frame = frame;
		}

		public AbsoluteSimulationFrame GetSimulationFrame() => Frame;

		public ICoherenceComponentData MergeWith(ICoherenceComponentData data, uint mask)
		{
			var other = (WorldPosition)data;
			if ((mask & 0x01) != 0)
			{
				Frame = other.Frame;
				value = other.value;
			}
			mask >>= 1;
			return this;
		}

		public static void Serialize(WorldPosition data, uint mask, IOutProtocolBitStream bitStream)
		{
			if (bitStream.WriteMask((mask & 0x01) != 0))
			{
				bitStream.WriteVector3((data.value.ToCoreVector3()), FloatMeta.NoCompression());
			}
			mask >>= 1;
		}

		public static (WorldPosition, uint, uint?) Deserialize(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
	
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}
			return (val, mask, null);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeBullet1_a15f0809f29cbf74c99418d4581ffc2c_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeBullet2_7ed8c4dd2d3496548b9b77dbfdabead6_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeCFX_Flash_a81ef04ac72436744a08ef117fa691b4_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeCFXR__char_32_Explosion__char_32_3__char_32___char_43___char_32_Text__char_32_1_e528427daa147834a9cc58aaf27ec7c2_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeEnemySpawner_deb987f02f3b9c5438adee8e7657523e_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeEnemy_a112f92c18af08a4cb206e6b4611b41f_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypeEl__char_32_Grenado_d2d154794ac855841a82a46b05bb7869_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}
		public static (WorldPosition, uint, uint?) DeserializeArchetypePlayer_0965159253ec9e3429357a3d7625b08f_WorldPosition_LOD0(InProtocolBitStream bitStream)
		{
			var mask = (uint)0;
			var val = new WorldPosition();
			if (bitStream.ReadMask())
			{
				val.value = (bitStream.ReadVector3(FloatMeta.NoCompression())).ToUnityVector3();
				mask |= 0b00000000000000000000000000000001;
			}

			return (val, mask, 0);
		}

		/// <summary>
		/// Resets byte array references to the local array instance that is kept in the lastSentData.
		/// If the array content has changed but remains of same length, the new content is copied into the local array instance.
		/// If the array length has changed, the array is cloned and overwrites the local instance.
		/// If the array has not changed, the reference is reset to the local array instance.
		/// Otherwise, changes to other fields on the component might cause the local array instance reference to become permanently lost.
		/// </summary>
		public void ResetByteArrays(ICoherenceComponentData lastSent, uint mask)
		{
			var last = lastSent as WorldPosition?;
	
		}
	}
}