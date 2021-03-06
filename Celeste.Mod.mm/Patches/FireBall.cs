﻿#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
#pragma warning disable CS0169 // The field is never used

using Celeste.Mod;
using Microsoft.Xna.Framework.Input;
using Monocle;
using MonoMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Celeste {
    class patch_FireBall : FireBall {

        private Vector2[] nodes;
        private float[] lengths;

        public patch_FireBall(Vector2[] nodes, int amount, int index, float offset, float speedMult)
            : base(nodes, amount, index, offset, speedMult) {
            // no-op. MonoMod ignores this - we only need this to make the compiler shut up.
        }

        [MonoModReplace]
        private Vector2 GetPercentPosition(float percent) {
            if (percent <= 0f)
                return nodes[0];

            if (percent >= 1f)
                return nodes[nodes.Length - 1];

            float lengthMax = lengths[lengths.Length - 1];
            float length = lengthMax * percent;
            int i;
            for (i = 0; i < lengths.Length - 1; i++)
                if (lengths[i + 1] > length)
                    break;

            // Edge case that currently has only been hit by Dournbrood by trying to create a "static FireBall."
            if (i == lengths.Length - 1)
                return nodes[0];

            float min = lengths[i] / lengthMax;
            float max = lengths[i + 1] / lengthMax;
            float lerp = Calc.ClampedMap(percent, min, max, 0f, 1f);
            return Vector2.Lerp(nodes[i], nodes[i + 1], lerp);
        }

    }
}
