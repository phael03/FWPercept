using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class EmotionRealSense : IEmotion
    {

        private int faceId;
        private float positionX;
        private float positionY;
        private float positionZ;
        private EmotionEnum emotion;

        public enum EmotionEnum
        {
            Anger = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_ANGER,
            Contempt = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_CONTEMPT,
            Disgust = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_DISGUST,
            Fear = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_FEAR,
            Joy = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_JOY,
            Sadness = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_SADNESS,
            Surprise = PXCMEmotion.EmotionData.Emotion.EMOTION_PRIMARY_SURPRISE,
            Positive = PXCMEmotion.EmotionData.Emotion.EMOTION_SENTIMENT_POSITIVE,
            Negative = PXCMEmotion.EmotionData.Emotion.EMOTION_SENTIMENT_NEGATIVE,
            Neutral = PXCMEmotion.EmotionData.Emotion.EMOTION_SENTIMENT_NEUTRAL,
            None
        }

        public EmotionRealSense(int FaceId,float PositionX,float PositionY,float PositionZ,EmotionEnum Emotion) 
        {
            faceId = FaceId;
            positionX = PositionX;
            positionY = PositionY;
            positionZ = PositionZ;
            emotion = Emotion;
        }

        public int GetFaceId()
        {
            return faceId;
        }

        public string GetDescription()
        {
            string em = "";
            switch (emotion)
            {
                case EmotionEnum.Anger:
                    em = "Anger";
                    break;
                case EmotionEnum.Contempt:
                    em = "Contempt";
                    break;
                case EmotionEnum.Disgust:
                    em = "Disgust";
                    break;
                case EmotionEnum.Fear:
                    em = "Fear";
                    break;
                case EmotionEnum.Joy:
                    em = "Joy";
                    break;
                case EmotionEnum.Sadness:
                    em = "Sadness";
                    break;
                case EmotionEnum.Surprise:
                    em = "Surprise";
                    break;
                case EmotionEnum.Positive:
                    em = "Positive";
                    break;
                case EmotionEnum.Negative:
                    em = "Negative";
                    break;
                case EmotionEnum.Neutral:
                    em = "Neutral";
                    break;
                case EmotionEnum.None:
                    em = null;
                    break;
                default:
                    break;
            }
            return em;
        }

        public float PositionX
        {
            get { return positionX; }
        }

        public float PositionY
        {
            get { return positionY; }
        }

        public float PositionZ
        {
            get { return positionZ; }
        }

        public EmotionEnum Emotion
        {
            get { return emotion; }
        }

    }
}
