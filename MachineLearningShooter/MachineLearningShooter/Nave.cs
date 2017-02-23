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
    public class Nave
    {
        //dados constantes da nave
        const int VIDA = 100;
        const int ESCUDO = 50;
        const int RAPIDEZ = 5;
        const int TIROS_POR_SEGUNDO = 5;
        const int TEMPO_TIROS = 1000/TIROS_POR_SEGUNDO;
        const int TEMPO_ARREFECIMENTO_ESCUDO_SEGUNDOS = 10;
        const int TEMPO_ARREFECIMENTO_ESCUDO = TEMPO_ARREFECIMENTO_ESCUDO_SEGUNDOS * 1000;


        //variaveis da nave
        int vidaActual, escudoActual, posicaoX, posicaoY, tempoTiros, tempoEscudo, posCentroX;
        bool escudoActivo, visivel, disparou, disable;
        SoundEffect explosao, disparo, reflectir;
        Jogador jogador;
        

        //texturas usadas pela a nave
        Texture2D texturaNave, texturaBala, texturaEscudo;

        //rectangulos de desenho
        Rectangle naveDrawRectangulo, escudoDrawRectangulo;

        public Nave(ContentManager content, SoundEffect[] sound, int centroX, int posicaoFixaY,Jogador j)
        {
            //carrega as texturas e sons
            LoadContent(content, sound);

            //inicializa os valores da nave
            vidaActual = VIDA;
            escudoActual = 0;
            posicaoX = centroX-texturaNave.Width/2;
            posCentroX = posicaoX;
            posicaoY = posicaoFixaY;
            escudoActivo = false;
            visivel = true;
            jogador = j;
            disparou = false;
            disable = false;
            tempoEscudo = 0;
            tempoTiros = 0;

            //cria um novo rectangulo de desenho para a nave e o escudo
            naveDrawRectangulo = new Rectangle(posicaoX, posicaoY, texturaNave.Width, texturaNave.Height);
            escudoDrawRectangulo = new Rectangle(centroX-texturaEscudo.Width/2, posicaoY-(texturaEscudo.Height-texturaNave.Height)/2,
                texturaEscudo.Width, texturaEscudo.Height);
            
        }

        public void Update(GameTime gametime, KeyboardState keyboard)
        {
            if ((visivel||disable) && Jogador.Jogador2==jogador)
            {
                if (keyboard.IsKeyDown(Keys.Right) && posicaoX + naveDrawRectangulo.Width + RAPIDEZ <= ConstantesDoJogo.WINDOW_WIDTH)
                {
                    naveDrawRectangulo.X += RAPIDEZ;
                    escudoDrawRectangulo.X += RAPIDEZ;
                }

                if (keyboard.IsKeyDown(Keys.Left) && posicaoX - RAPIDEZ >= 0)
                {
                    naveDrawRectangulo.X -= RAPIDEZ;
                    escudoDrawRectangulo.X -= RAPIDEZ;
                }

                if (keyboard.IsKeyDown(Keys.Space) && vidaActual > 0 && disparou==false)
                {
                    disparou = true;
                    Jogo.AdicionarBala(new Bala(texturaBala, jogador, posicaoX + naveDrawRectangulo.Width / 2 - texturaBala.Width / 2,
                        posicaoY, true));
                }
                
                if (disparou == true)
                    tempoTiros += gametime.ElapsedGameTime.Milliseconds;

                if (tempoTiros >= TEMPO_TIROS)
                {
                    disparou = false;
                    tempoTiros = 0;
                }

                if (keyboard.IsKeyDown(Keys.C) && tempoEscudo == 0 && escudoActivo == false)
                {
                    escudoActivo = true;
                    escudoActual = ESCUDO;
                    tempoEscudo = TEMPO_ARREFECIMENTO_ESCUDO;
                }

                if (tempoEscudo > 0)
                    tempoEscudo -= gametime.ElapsedGameTime.Milliseconds;

                

                
            }

            if ((visivel || disable) && Jogador.Jogador1 == jogador)
            {
                if (keyboard.IsKeyDown(Keys.L) && posicaoX + naveDrawRectangulo.Width + RAPIDEZ <= ConstantesDoJogo.WINDOW_WIDTH)
                {
                    naveDrawRectangulo.X += RAPIDEZ;
                    escudoDrawRectangulo.X += RAPIDEZ;
                }

                if (keyboard.IsKeyDown(Keys.K) && posicaoX - RAPIDEZ >= 0)
                {
                    naveDrawRectangulo.X -= RAPIDEZ;
                    escudoDrawRectangulo.X -= RAPIDEZ;  
                }

                if (keyboard.IsKeyDown(Keys.M) && vidaActual > 0 && disparou == false)
                {
                    disparou = true;
                    Jogo.AdicionarBala(new Bala(texturaBala, jogador, posicaoX + naveDrawRectangulo.Width/2 - texturaBala.Width/2, 
                        posicaoY+ naveDrawRectangulo.Height - texturaBala.Height, false));
                }

                if (disparou == true)
                    tempoTiros += gametime.ElapsedGameTime.Milliseconds;

                if (tempoTiros >= TEMPO_TIROS)
                {
                    disparou = false;
                    tempoTiros = 0;
                }

                if (keyboard.IsKeyDown(Keys.N) && tempoEscudo == 0 && escudoActivo == false)
                {
                    escudoActivo = true;
                    escudoActual = ESCUDO;
                    tempoEscudo = TEMPO_ARREFECIMENTO_ESCUDO;
                }

                if (tempoEscudo > 0)
                    tempoEscudo -= gametime.ElapsedGameTime.Milliseconds;
            }

            posicaoX = naveDrawRectangulo.X;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texturaNave, naveDrawRectangulo, Color.White);
            if (escudoActivo)
                spriteBatch.Draw(texturaEscudo, escudoDrawRectangulo, Color.White);

        }

        public bool Visivel
        {
            set { visivel = value; }
            get { return visivel; }

        }

        public int Vida
        {
            get { return vidaActual; }
            set { vidaActual = value; }
        }

        public int PercentagemVida
        {
            get { return (vidaActual * 100 ) / VIDA; }
        }

        public int PercentagemVidaBarra
        {
            get { return ((vidaActual*ConstantesDoJogo.WINDOW_WIDTH) / VIDA); }
        }

        public int Escudo
        {
            get { return escudoActual; }
            set { escudoActual = value; }
        }

        public bool EscudoActivo
        {
            get { return escudoActivo; }
            set { escudoActivo = value; }
        }

        public int PercentagemEscudo
        {
            get { return (escudoActual * 100 ) / ESCUDO; }
        }

        public int PercentagemEscudoBarra
        {
            get { return ((escudoActual * ConstantesDoJogo.WINDOW_WIDTH) / ESCUDO); }
        }

        public int TempoEscudo
        {
            get { return (tempoEscudo / 1000); }
        }

        public Rectangle Forma
        {
            get { return naveDrawRectangulo; }
        }

        public Rectangle EscudoForma
        {
            get { return escudoDrawRectangulo; }
        }

        public Texture2D TexturaNave
        {
            get { return texturaNave; }
        }

        public Texture2D TexturaEscudo
        {
            get{ return texturaEscudo;}
        }

        public bool Desactivado
        {
            set { disable = value; }
        }

        public void Reset()
        {
            vidaActual = VIDA;
            escudoActual = 0;
            posicaoX = posCentroX;
            escudoActivo = false;
            visivel = true;
            disparou = false;
            tempoEscudo = 0;
            tempoTiros = 0;
            disable = false;
        }

        private void LoadContent(ContentManager content, SoundEffect[] sound)
        {
            //carrega as texturas
            texturaNave = content.Load<Texture2D>("nave");
            texturaEscudo = content.Load<Texture2D>("escudo");
            texturaBala = content.Load<Texture2D>("bala");

            //carrega os sons

        }

    }
}
