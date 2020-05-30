package com.example.segurados.view.ui.jogar;


import android.os.Bundle;
import android.os.Handler;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.cardview.widget.CardView;
import androidx.core.content.ContextCompat;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.segurados.Interface.ComunicadorJ;
import com.example.segurados.R;
import com.example.segurados.adapter.OpcaoAdapter;
import com.example.segurados.model.Opcao;
import com.example.segurados.model.Pergunta;
import com.example.segurados.model.PontosUsuarioViewModel;
import com.example.segurados.model.Tematica;
import com.example.segurados.model.UsuarioHasPergunta;
import com.example.segurados.model.UsuarioViewModel;
import com.example.segurados.service.UsuarioHasPerguntaService;
import com.example.segurados.util.Util;
import com.example.segurados.view.ui.perfil.PerfilFragment;
import com.github.lzyzsd.circleprogress.DonutProgress;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Objects;

import io.realm.Realm;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.example.segurados.util.Util.status;

/**
 * A simple {@link Fragment} subclass.
 */
public class PerguntaFragment extends Fragment {
  private RecyclerView mRecyclerView;
  //  private AdapterOpcao adapterOpcao;
  private TextView txtPergunta;
  private CardView cardView;
  private TextView txtLog;
  private TextView txtPontos;
  private ImageView imgResposta;
  private Button btnContinuar;
  private Button btnFinalizar;
  private Pergunta pergunta;
  // detalhe de interface
  private ComunicadorJ comunicador;
  private LinearLayout linearLayout;
  private int tempo;
  private int progressStatus;
  private DonutProgress donutProgress;
  private ProgressBar progressBar;
  private int verificaProgress;
  private ArrayList<Opcao> opcoes;
  private UsuarioViewModel user;
  private UsuarioHasPergunta hP;
  private  Realm realm;
  public PerguntaFragment() {
    // Required empty public constructor
  }

  @Override
  public void onActivityCreated(Bundle savedInstacedState){
    super.onActivityCreated(savedInstacedState);

    comunicador = (ComunicadorJ) getActivity();
  }


  @Override
  public View onCreateView(LayoutInflater inflater, ViewGroup container,
                           Bundle savedInstanceState) {
    // Inflate the layout for this fragment
    View v =  inflater.inflate(R.layout.fragment_pergunta, container, false);
    realm = Realm.getDefaultInstance();

    user = realm.where(UsuarioViewModel.class).findAll().first();

    int key = -1;
    if((this.getArguments() != null))
      key = this.getArguments().getInt("idPergunta");

    if(key != -1){
      pergunta = realm.where(Pergunta.class).equalTo("idPergunta", key).findFirst();
    }

    txtPergunta = v.findViewById(R.id.txt_pergunta_pg);
    // ----------------------------------------------------------------------
    if (pergunta != null) {
      // referencia dos itens do carView
      cardView = v.findViewById(R.id.mycard_pg);
      txtPontos = v.findViewById(R.id.txt_pontuacao_obtida_pg);
      txtLog = v.findViewById(R.id.txt_log_resposta_pg);
      imgResposta = v.findViewById(R.id.img_resposta_pg);
      btnContinuar = v.findViewById(R.id.btn_continuar_pg);
      btnFinalizar = v.findViewById(R.id.btn_finalizar_pg);
      // refenrencia dos itens do progressbar
      donutProgress = v.findViewById(R.id.donut_progress_pg);
      // recycler
      mRecyclerView = v.findViewById(R.id.rcv_opcao_pg);
      // pergunta
      txtPergunta = v.findViewById(R.id.txt_pergunta_pg);
      progressBar = v.findViewById(R.id.progress_pg);
      linearLayout = v.findViewById(R.id.linear);

      // ------------------------------------------------------------------
      opcoes = new ArrayList<>();
      txtPergunta.setText(pergunta.getQuestao());

      Opcao op = new Opcao(pergunta.getAlternativa1());
      opcoes.add(op);
      op = new Opcao(pergunta.getAlternativa2());
      opcoes.add(op);
      /*
        op = new Opcao(pergunta.getAlternativa3());
        opcoes.add(op);
        op = new Opcao(pergunta.getAlternativa4());
        opcoes.add(op);
       */
      // ---------------------------------------------------------------------
      //circle progress

      donutProgress.setFinishedStrokeColor(ContextCompat.getColor(Objects.requireNonNull(getActivity()), R.color.colorPrimaryDark));
      donutProgress.setTextColor(ContextCompat.getColor(Objects.requireNonNull(getActivity()), R.color.colorAccent));
      donutProgress.setKeepScreenOn(true);
      donutProgress.setSuffixText("s");
      //---------------------------------------------------------------------
      //Componentes do Recicler
      RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getContext(), 1);
      mRecyclerView.setHasFixedSize(true);
      mRecyclerView.setLayoutManager(layoutManager);

      OpcaoAdapter opcaoAdapter= new OpcaoAdapter(opcoes, getContext());
      mRecyclerView.setAdapter(opcaoAdapter);

      // -----------------------------------------------------------------------
      progressStatus =  pergunta.getTempo();
      int tmax  = pergunta.getTempo();
      donutProgress.setMax(tmax);
      // ProgressBar que conta o tempo de resposta do usuário
      Handler handler = new Handler();
      new Thread(new Runnable() {
        public void run() {
          while (progressStatus >= 0) {
            handler.post(new Runnable() {
              public void run() {
                donutProgress.setProgress(progressStatus);
                verificaProgress = progressStatus;
                if (progressStatus == 0) {
                  menuOpcao(2);
                }
              }
            });
            try {
              // Sleep for 1000 milliseconds.
              Thread.sleep(1000);
            } catch (InterruptedException e) {
              e.printStackTrace();
            }
            progressStatus -= 1;
          }
        }
      }).start();

      hP = new UsuarioHasPergunta();
      // -----------------------------------------------------------------------------
      opcaoAdapter.setOnItemClickListener(position -> {
        tempo = progressStatus;
        progressStatus = 0;
        if (tempo > 0) {
          // verificando se a opcao escolhida foi a correta
          if (position  == pergunta.getOpcaoCorreta() -1) {
            menuOpcao(0);
            /* gravaResposta();  aqui o momento em que eu gravo a resposta,
             * quando o usuário clica, mas só grava se a resposta for correta */

            PontosUsuarioViewModel pvm = realm.where(PontosUsuarioViewModel.class).equalTo("tematica.idTematica", pergunta.getTematicaIdTematica()).findFirst();
            if(pvm != null) {
                realm.beginTransaction();
                pvm.setPontos(pergunta.getPontuacao() + pvm.getPontos());
                realm.insertOrUpdate(pvm);
                realm.commitTransaction();
            }
            else{
                realm.beginTransaction();
                pvm = realm.createObject(PontosUsuarioViewModel.class, PontosUsuarioViewModel.autoIncrementId());
                pvm.setPontos(pergunta.getPontuacao());
                Tematica tema = realm.where(Tematica.class).equalTo("idTematica", pergunta.getTematicaIdTematica()).findFirst();
                pvm.setTematica(tema);
                realm.commitTransaction();
            }
          }else
            menuOpcao(1); /* resposta errada */

        }else{
          menuOpcao(2); /* tempo esgotado */
        }


      });
      // -------------------------------------------------------------------------------
      btnContinuar.setOnClickListener(new View.OnClickListener() {
        @Override
        public void onClick(View v) {
          linearLayout.setVisibility(View.GONE);
          progressBar.setVisibility(View.VISIBLE);
          try {
            Thread.sleep(1000);

          } catch (InterruptedException e) {
            e.printStackTrace();
          }
          // ------------------------------------------------------------------
          // removendo o primeiro elemento do array, assim sempre podemos selecionar a pergunta de indice 0
          // Ir pra roleta de novo
          status = false;
          FragmentManager fm = getActivity().getSupportFragmentManager();
          FragmentTransaction ft = fm.beginTransaction();
          ft.replace(R.id.containerFragment, new JogarFragment()).commit();

        }
      });



      btnFinalizar.setOnClickListener(new View.OnClickListener() {
        @Override
        public void onClick(View v) {
          // aqui a requisicão
          linearLayout.setVisibility(View.GONE);
          progressBar.setVisibility(View.VISIBLE);
          try {
            Thread.sleep(1000);
          } catch (InterruptedException e) {
            e.printStackTrace();
          }
          // ------------------------------------------------------------------
          status = false;
          comunicador.fimJogo(true);
        }
      });
      // -------------------------------------------------------------------------------
    }else {
      //donutProgress.setVisibility(View.GONE);
      txtPergunta.setText("As perguntas acabaram, tente novamente mais tarde!");
    }

    return v;
  }

  public void menuOpcao(int resposta){
    if(resposta == 0)
      hP.setAcertou(1);
    else
      hP.setAcertou(0);

    realm.beginTransaction();
    hP.setIdPergunta(pergunta.getIdPergunta());
    hP.setIdUsuario(user.getIdUsuario());
    hP.createUniqueKey();
    user.setQtdQuestoes(user.getQtdQuestoes()+1);
    realm.insertOrUpdate(user);
    realm.insertOrUpdate(hP);
    realm.commitTransaction();
    realm.close();
    if(Util.checkInternet(getActivity())){
      System.out.println(hP.getIdPergunta() + " " + hP.getIdUsuario() +" " + hP.isAcertou());
      if(hP != null)
        new Util.AddResposta(getActivity(), hP, user.getToken()).start();
      else
        Toast.makeText(getActivity(), "Erro ", Toast.LENGTH_LONG).show();
    }
    else{
      Toast.makeText(getActivity(), " outro", Toast.LENGTH_LONG).show();

      File f = new File(getActivity().getFilesDir()+"/filaRespostas.txt");
      try {
        BufferedWriter bw = new BufferedWriter(new FileWriter(f, true));
        bw.write(hP.getIdUsuario() + ";" + hP.getIdPergunta() + ";" + hP.isAcertou());
        bw.newLine();
        bw.close();
      } catch (IOException e) {
        e.printStackTrace();
      }
    }
    if (cardView.getVisibility() == View.GONE){
      if (resposta == 0){
        txtLog.setText("Resposta Correta!");
        txtPontos.setText("Pontuacao: "+ pergunta.getPontuacao());
        imgResposta.setImageResource(R.drawable.certo);
      }else if (resposta == 1){
        txtLog.setText("Resposta Errada!");
        imgResposta.setImageResource(R.drawable.errado);
        txtPontos.setText("Pontuação: "+0);
      }else if (resposta == 2){
        txtLog.setText("Seu tempo Acabou!");
        imgResposta.setImageResource(R.drawable.errado);
        txtPontos.setText("Pontuação: "+0);
      }
      cardView.setVisibility(View.VISIBLE);
    }
      status = false;

  }

}
