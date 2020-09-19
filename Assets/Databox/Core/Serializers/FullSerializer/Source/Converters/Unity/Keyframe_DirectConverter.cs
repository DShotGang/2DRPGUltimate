#if !NO_UNITY
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Databox.FullSerializer {
    partial class fsConverterRegistrar {
        public static Internal.DirectConverters.Keyframe_DirectConverter Register_Keyframe_DirectConverter;
    }
}

namespace Databox.FullSerializer.Internal.DirectConverters {
    public class Keyframe_DirectConverter : fsDirectConverter<Keyframe> {
        protected override fsResult DoSerialize(Keyframe model, Dictionary<string, fsData> serialized) {
            var result = fsResult.Success;

            result += SerializeMember(serialized, null, "time", model.time);
            result += SerializeMember(serialized, null, "value", model.value);
	        //result += SerializeMember(serialized, null, "tangentMode", model.tangentMode);
            result += SerializeMember(serialized, null, "inTangent", model.inTangent);
	        result += SerializeMember(serialized, null, "outTangent", model.outTangent);
	        #if UNITY_2018_OR_NEWER
	        result += SerializeMember(serialized, null, "outWeight", model.outWeight);
	        result += SerializeMember(serialized, null, "inWeight", model.inWeight);
	        result += SerializeMember(serialized, null, "weighedMode", model.weightedMode);
			#endif
            return result;
        }

        protected override fsResult DoDeserialize(Dictionary<string, fsData> data, ref Keyframe model) {
            var result = fsResult.Success;

            var t0 = model.time;
            result += DeserializeMember(data, null, "time", out t0);
            model.time = t0;

            var t1 = model.value;
            result += DeserializeMember(data, null, "value", out t1);
            model.value = t1;

            //var t2 = model.tangentMode;
            //result += DeserializeMember(data, null, "tangentMode", out t2);
            //model.tangentMode = t2;

	        var t2 = model.inTangent;
	        result += DeserializeMember(data, null, "inTangent", out t2);
	        model.inTangent = t2;

	        var t3 = model.outTangent;
	        result += DeserializeMember(data, null, "outTangent", out t3);
	        model.outTangent = t3;
	         #if UNITY_2018_OR_NEWER
	        var t4 = model.outWeight;
	        result += DeserializeMember(data, null, "outWeight", out t4);
	        model.outWeight = t4;

	        var t5 = model.inWeight;
	        result += DeserializeMember(data, null, "inWeight", out t5);
	        model.inWeight = t5;

	        var t6 = model.weightedMode;
	        result += DeserializeMember(data, null, "weightedMode", out t6);
	        model.weightedMode = t6;
			#endif
            return result;
        }

        public override object CreateInstance(fsData data, Type storageType) {
            return new Keyframe();
        }
    }
}
#endif