using System.Collections.Generic;

namespace Yata.CoreNode
{
    public class Envelope
    {
        public struct EnvelopePoint
        {
            public float t;
            public float value;
            public EnvelopePoint(float t, float v)
            {
                this.t = t;
                this.value = v;
            }
        }

        public class EnvelopeLine : List<EnvelopePoint>
        {
            public string id;
            public EnvelopeLine(string name)
            {
                id = name;
            }
            public float GetValue(float t)
            {
                if (Count == 0) return 0;
                if (Count == 1) return this[0].value;

                int index = 0;
                while (index < Count - 1 && t >= this[index].t)
                {
                    index++;
                }
                index--;
                EnvelopePoint p1 = this[index];
                EnvelopePoint p2 = this[index + 1];

                float newT = 1.0f / (p2.t - p1.t) * (t - p1.t);
                return Utils.Lerp(p1.value, p2.value, newT);
            }
            public void AddPoint(EnvelopePoint p)
            {
                Add(p);
                DoSort();
            }
            public void DoSort()
            {
                Sort(delegate(EnvelopePoint p1, EnvelopePoint p2)
                {
                    int comp = p1.t.CompareTo(p2.t);
                    if (comp == 0)
                    {
                        return p2.t.CompareTo(p1.t);
                    }
                    return comp;
                });
            }
        }

        private List<EnvelopeLine> lines = new List<EnvelopeLine>();

        public Envelope()
        {
        }

        public EnvelopeLine AddLine(string id)
        {
            EnvelopeLine line = new EnvelopeLine(id);
            lines.Add(line);
            return line;
        }

        public float GetValue(string id, float t)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].id == id)
                {
                    return lines[i].GetValue(t);
                }
            }
            return 0;
        }

        public List<EnvelopeLine> GetLines()
        {
            return lines;
        }
    }
}
