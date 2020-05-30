package com.example.segurados.view.ui.jogar;

import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import com.adefruandta.spinningwheel.SpinningWheelView;
import com.bumptech.glide.Glide;
import com.bumptech.glide.load.DataSource;
import com.bumptech.glide.load.engine.GlideException;
import com.bumptech.glide.request.RequestListener;
import com.bumptech.glide.request.target.Target;
import com.example.segurados.R;
import com.example.segurados.model.Pergunta;
import com.example.segurados.model.PontosUsuarioViewModel;
import com.example.segurados.model.Tematica;
import com.example.segurados.model.UsuarioHasPergunta;
import com.example.segurados.model.UsuarioViewModel;
import com.example.segurados.service.PerguntaService;
import com.example.segurados.service.TematicaService;
import com.example.segurados.util.Util;
import com.github.mikephil.charting.data.PieEntry;
import com.github.mikephil.charting.utils.ColorTemplate;

import java.util.ArrayList;
import java.util.List;

import de.hdodenhof.circleimageview.CircleImageView;
import io.realm.Realm;
import io.realm.RealmResults;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.example.segurados.util.Util.status;

/**
 * A simple {@link Fragment} subclass.
 */
public class JogarFragment extends Fragment implements SpinningWheelView.OnRotationListener<String>  {
    private SpinningWheelView wheelView;
    private Button btnGirar;
    private ProgressBar pbCarregando;
    private TextView txtCarregando;
    private List<Tematica> tematicaList;
    private TextView txtNomeUsuario;
    private TextView pontosUsuario;
    private TextView qtdPerguntas;
    private CircleImageView imgPerfil;
    private TematicaService tematicaService;
    private Realm realm;
    private RealmResults<UsuarioViewModel> user;
    private RealmResults<UsuarioHasPergunta> hasPerguntas;
    private final int MIN_SPIN = 60;
    private final int MAX_SPIN = 180;

    public JogarFragment() {
        // Required empty public constructor
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v = inflater.inflate(R.layout.fragment_jogar, container, false);

        wheelView = v.findViewById(R.id.wheel_j);
        btnGirar = v.findViewById(R.id.btn_girar_j);
        pbCarregando = v.findViewById(R.id.progress_bar_j);
        txtCarregando = v.findViewById(R.id.txt_carregando_j);
        txtNomeUsuario = v.findViewById(R.id.nome_jogador_j);
        pontosUsuario = v.findViewById(R.id.pontos_jogador_j);
        qtdPerguntas = v.findViewById(R.id.qtd_questoes_j);
        imgPerfil = v.findViewById(R.id.img_perfil_j);
        realm = Realm.getDefaultInstance();
        user = realm.where(UsuarioViewModel.class).findAll();
        hasPerguntas = realm.where(UsuarioHasPergunta.class).equalTo("idUsuario", user.first().getIdUsuario()).findAll();

        txtNomeUsuario.setText(user.first().getNome());
        qtdPerguntas.setText(user.first().getQtdQuestoes() + " " + getString(R.string.qtd_questoes));
        Glide.with(getActivity())
                .load(user.first().getPerfil())
                .listener(new RequestListener<Drawable>() {
                    @Override
                    public boolean onLoadFailed(@Nullable GlideException e, Object model, Target<Drawable> target, boolean isFirstResource) {
                        return false;
                    }

                    @Override
                    public boolean onResourceReady(Drawable resource, Object model, Target<Drawable> target, DataSource dataSource, boolean isFirstResource) {
                        return false;
                    }
                })
                .into(imgPerfil);      //classe que pega a foto da url e seta o imageView
        RealmResults<PontosUsuarioViewModel> pU = realm.where(PontosUsuarioViewModel.class).findAll();
        int pontos = 0;
        for(PontosUsuarioViewModel us : pU){
            pontos += us.getPontos();
        }
        pontosUsuario.setText(pontos + " " + getString(R.string.pontoss));
        status = false;
        // se tiver internet carrega tudo da api
        if(Util.checkInternet(getActivity())){
            tematicaService = TematicaService.retrofit.create(TematicaService.class);
            final Call<List<Tematica>> call = tematicaService.getTematicas("bearer " + user.first().getToken());
            loadDataTematic(call);
        }

        // ve se tem algo no bd realm
        else{
            tematicaList =  realm.where(Tematica.class).findAll();
            RealmResults<Pergunta> perguntaList = realm.where(Pergunta.class).findAll();
            Pergunta perg = checkQuests(perguntaList);
            setWheelSpin();

            if(perg == null) {
                Toast.makeText(getActivity(), "Você finalizou o jogo, em breve adicionaremos mais perguntas.", Toast.LENGTH_LONG).show();
            }
        }

        return v;
    }

    @Override
    public void onRotation() {
        //Log.d("XXXX", "On Rotation");
    }

    @Override
    public void onStopRotation(String item) {
        Realm realm = Realm.getDefaultInstance();
        Toast.makeText(getActivity(), item, Toast.LENGTH_LONG).show();
        int pos = -1;
        for(Tematica t : tematicaList){
            if(t.getTitulo().equals(item)){
                pos = t.getIdTematica();
            }
        }
        // aqui pega o tema que foi parou na roda
        RealmResults<Pergunta> perguntaList = realm.where(Pergunta.class).equalTo("tematicaIdTematica", pos).findAll();
        Pergunta perg = checkQuests(perguntaList);

        if(perg != null) {
            status = true;
            Bundle bundle = new Bundle();
            FragmentManager fm = getActivity().getSupportFragmentManager();
            FragmentTransaction ft = fm.beginTransaction();
            PerguntaFragment perguntaFragment = new PerguntaFragment();
            bundle.putInt("idPergunta", perg.getIdPergunta());
            perguntaFragment.setArguments(bundle);
            ft.replace(R.id.containerFragment, perguntaFragment);
            ft.commit();
        }
        else{
            Toast.makeText(getActivity(), "Gire de novo, pois esse tema já esgostou as perguntas!", Toast.LENGTH_LONG).show();
        }

    }

    private Pergunta checkQuests(RealmResults<Pergunta> perguntaList){
        Integer ph[] = new Integer[hasPerguntas.size()];
        for(int i = 0; i < ph.length; i++)
            ph[i] = hasPerguntas.get(i).getIdPergunta();

        Pergunta perg = perguntaList.where().not().in("idPergunta", ph).findFirst();
        return perg;
    }
    private void loadDataTematic(Call<List<Tematica>> call){
        call.enqueue(new Callback<List<Tematica>>() {
            @Override
            public void onResponse(Call<List<Tematica>> call, Response<List<Tematica>> response) {
                int code = response.code();
                realm = Realm.getDefaultInstance();
                tematicaList = response.body();
                if(code == 200){
                    realm.beginTransaction();
                    for(Tematica t : tematicaList){
                        realm.copyToRealmOrUpdate(t);
                    }
                    realm.commitTransaction();
                    PerguntaService perguntaService = PerguntaService.retrofit.create(PerguntaService.class);
                    Call<List<Pergunta>> callT = perguntaService.getPerguntas("bearer " + user.first().getToken());
                    loadQuests(callT);
                }else{

                    Toast.makeText(getContext(),"Falhou",
                            Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Tematica>> call, Throwable t) {

            }
        });
    }

    public void setWheelSpin(){
        List<String> tituloTemas = new ArrayList<>();
        for(Tematica t : tematicaList)
            tituloTemas.add(t.getTitulo());

        wheelView.setItems(tituloTemas);
        wheelView.setOnRotationListener(this);
        wheelView.setColors(ColorTemplate.PASTEL_COLORS);
        wheelView.setWheelTextColor(Color.WHITE);
        pbCarregando.setVisibility(View.GONE);
        txtCarregando.setVisibility(View.GONE);
        wheelView.setVisibility(View.VISIBLE);
        btnGirar.setVisibility(View.VISIBLE);

        btnGirar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // max angle 50
                // duration 10 second
                // every 50 ms rander rotation
                // min angulo, max angulo para girar, se n sempr vai ser previsivel
                int random_spin = (int)(Math.random() * (MAX_SPIN - MIN_SPIN + 1) + MIN_SPIN );

                wheelView.rotate(random_spin, random_spin * 20, 50);
            }
        });
    }

    private void loadQuests(Call<List<Pergunta>> call) {
        call.enqueue(new Callback<List<Pergunta>>() {
            @Override
            public void onResponse(Call<List<Pergunta>> call, Response<List<Pergunta>> response) {
                int code = response.code();

                if (code == 200) {
                    List<Pergunta> perguntas = response.body();
                    realm.beginTransaction();
                    for(Pergunta p : perguntas) {
                        realm.copyToRealmOrUpdate(p);
                    }
                    realm.commitTransaction();
                    setWheelSpin();
                    RealmResults perguntaList = realm.where(Pergunta.class).findAll();
                    Pergunta perg = checkQuests(perguntaList);
                    if(perg == null) {
                        Toast.makeText(getActivity(), "Você finalizou o jogo, em breve adicionaremos mais perguntas.", Toast.LENGTH_LONG).show();
                    }
                } else {

                    Toast.makeText(getActivity(), "Falhou",
                            Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<List<Pergunta>> call, Throwable t) {
                t.printStackTrace();
            }
        });

    }
}
