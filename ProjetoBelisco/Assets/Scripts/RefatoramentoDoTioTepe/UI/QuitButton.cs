 namespace RefatoramentoDoTioTepe
 {
     public class QuitButton : AbstractButton<QuitButton>
     {
         protected override void OnEnable()
         {
             _instance = this;
             base.OnEnable();
         }
     }
 }