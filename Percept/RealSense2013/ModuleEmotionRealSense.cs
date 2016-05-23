using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class ModuleEmotionRealSense : IModule, IModuleEmotion
    {

        public ModuleEmotionRealSense()
            : base()
        {
            
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "EmotionRealSense";
        }

        public int GetIdModule()
        {
            return 6;
        }

        public IEmotion GetEmotionFromFrame(EventArgs e) 
        {
            PXCMEmotion emot = ((EmotionRecognitionSenseEventArgs)e).ObjEmotion;
            EmotionRealSense emotion = null;

            UInt32 num_faces = emot.QueryNumFaces();

            for (UInt32 fid = 0; fid < num_faces; fid++)
            {
                // retrieve all estimation data
                PXCMEmotion.EmotionData[] arrData=new PXCMEmotion.EmotionData[10];
                emot.QueryAllEmotionData(fid, arrData);
                
                // find the emotion with maximum evidence
                int idx_emotion = 0;
                int max_evidence = arrData[0].evidence;
                
                for (int k=1; k<10; k++) 
                {
                    if (arrData[k].evidence < max_evidence) continue;
                    max_evidence = arrData[k].evidence;
                    idx_emotion=k;
                }

                if (arrData[idx_emotion].intensity > 0.4) 
                {
                    EmotionRealSense.EmotionEnum emotionEnumAux = (EmotionRealSense.EmotionEnum)arrData[idx_emotion].eid;                    
                    emotion = new EmotionRealSense((int)arrData[idx_emotion].fid, arrData[idx_emotion].rectangle.x, arrData[idx_emotion].rectangle.y, arrData[idx_emotion].rectangle.w, emotionEnumAux);
                    break;
                }
            }
            return emotion;
        }

        public List<EmotionRealSense> GetEmotions(EmotionRecognitionSenseEventArgs e)
        {
            PXCMEmotion emot = e.ObjEmotion;
            EmotionRealSense emotion = null;
            List<EmotionRealSense> list = new List<EmotionRealSense>();

            UInt32 num_faces = emot.QueryNumFaces();

            for (UInt32 fid = 0; fid < num_faces; fid++)
            {
                // retrieve all estimation data
                PXCMEmotion.EmotionData[] arrData = new PXCMEmotion.EmotionData[10];
                emot.QueryAllEmotionData(fid, arrData);

                // find the emotion with maximum evidence
                int idx_emotion = 0;
                int max_evidence = arrData[0].evidence;

                for (int k = 1; k < 10; k++)
                {
                    if (arrData[k].evidence < max_evidence) continue;
                    max_evidence = arrData[k].evidence;
                    idx_emotion = k;
                }

                if (arrData[idx_emotion].intensity > 0.4)
                {
                    EmotionRealSense.EmotionEnum emotionEnumAux = (EmotionRealSense.EmotionEnum)arrData[idx_emotion].eid;
                    emotion = new EmotionRealSense((int)arrData[idx_emotion].fid, arrData[idx_emotion].rectangle.x, arrData[idx_emotion].rectangle.y, arrData[idx_emotion].rectangle.w, emotionEnumAux);
                    list.Add(emotion);
                }
            }
            return list;
        }
    }
}
