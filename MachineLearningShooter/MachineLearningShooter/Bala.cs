using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MachineLearningShooter
{
    public class Bala
    {
        //valores constantes das caracteristicas da bala
        const int DANO = 10;
        const int VELOCIDADE = 5;
        const int TEMPO_POR_FRAME_MILISEGUNDO = 100;
        const int TEMPO_TOTAL_FRAME_MILISEGUNDO = 1000;

        //variáveis da posição da bala no ecrã e se visivel
        int posX, posY;
        bool visivel, paraCima;

        //enumerador de quem pertence a bala
        Jogador balaPertence;

        //textura da bala
        Texture2D texturaBala;
        Rectangle drawBala;//, sourceBala;

        public Bala(Texture2D textura, Jogador jogador, int posiX, int posiY, bool direccaoCima)
        {
            texturaBala = textura;
            balaPertence = jogador;
            posX = posiX;
            posY = posiY;
            paraCima = direccaoCima;
            drawBala = new Rectangle(posiX, posiY, texturaBala.Width, texturaBala.Height);
            visivel = true;

        }

        public void Update(GameTime gameTime)
        {
            if (paraCima == true)
            {
                posY -= VELOCIDADE;
                drawBala.Y = posY;
            }
            if (paraCima == false)
            {
                posY += VELOCIDADE;
                drawBala.Y = posY;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturaBala, drawBala, Color.White);
        }

        public int X
        {
            get { return posX; }
        }

        public int Y
        {
            get { return posY; }
        }

        public bool Visivel
        {
            get { return visivel; }
            set { visivel = value; }
        }

        public int Dano
        {
            get { return DANO; }
        }

        public Jogador Pertence
        {
            get { return balaPertence; }
        }

        public Rectangle Forma
        {
            get { return drawBala; }
        }

        public Texture2D TexturaBala
        {
            get {return texturaBala; }
        }
    }
}
