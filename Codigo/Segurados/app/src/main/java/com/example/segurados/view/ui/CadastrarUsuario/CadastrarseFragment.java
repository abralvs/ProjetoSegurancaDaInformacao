package com.example.segurados.view.ui.CadastrarUsuario;

import android.app.ProgressDialog;
import android.content.Context;
import android.os.Bundle;

import androidx.cardview.widget.CardView;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;

import android.os.Handler;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ScrollView;
import android.widget.TextView;
import com.example.segurados.Interface.Comunicador;
import com.example.segurados.R;
import com.example.segurados.model.Usuario;
import com.example.segurados.model.UsuarioViewModel;
import com.example.segurados.service.AuthenticateService;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * A simple {@link Fragment} subclass.
 */
public class CadastrarseFragment extends Fragment {
    private Comunicador comunicador;
    private AutoCompleteTextView edtNome;
    private AutoCompleteTextView edtUsuario;
    private EditText edtSenha;
    private EditText edtRepitaSenha;
    private Button btnCadastrar;
    private TextView txtMensagemErro;
    private ProgressDialog dialog;
    private CardView cardMensagm;
    private ScrollView scroll;


    public CadastrarseFragment(Context ctx) {
        comunicador = (Comunicador) ctx;
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        inflater.inflate(R.menu.layout_fechar, menu);
        super.onCreateOptionsMenu(menu, inflater);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id) {
            case R.id.action_back:

                fechaCadastro();

                return true;
        }

        return false;
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_cadastrarse, container, false);
        setHasOptionsMenu(true);


        edtNome = v.findViewById(R.id.edtNome);
        edtUsuario = v.findViewById(R.id.edtUsuario);
        edtSenha = v.findViewById(R.id.edtSenha);
        edtRepitaSenha = v.findViewById(R.id.edtRepitaSenha);
        btnCadastrar = v.findViewById(R.id.btnCadastrar);
        txtMensagemErro = v.findViewById(R.id.txtMensagemErro);
        cardMensagm = v.findViewById(R.id.cardMensagem);
        scroll = v.findViewById(R.id.scroll);

        btnCadastrar.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                mostraDialog();

                String senha1  = edtSenha.getText().toString();
                String senha2  = edtRepitaSenha.getText().toString();
                String nome    = edtNome.getText().toString();
                String usuario = edtUsuario.getText().toString();
                String perfil  = "https://icons-for-free.com/iconfiles/png/512/headset+male+man+support+user+young+icon-1320196267025138334.png";

                if (!senha1.equals("") && !senha2.equals("") &&
                    !nome.equals("")   && !usuario.equals("") &&
                    (usuario.length() >= 3) && (nome.length() >= 3) &&
                     (usuario.indexOf(" ") == -1)) {

                        if (senha1.equals(senha2) && senha1.length() >= 8) {

                            Usuario user = new Usuario(nome, usuario, perfil, senha1);

                            AuthenticateService auth = AuthenticateService.retrofit.create(AuthenticateService.class);
                            final Call<UsuarioViewModel> call = auth.register(user);

                            call.enqueue(new Callback<UsuarioViewModel>() {
                                @Override
                                public void onResponse(Call<UsuarioViewModel> call, Response<UsuarioViewModel> response) {
                                    int code = response.code();

                                    if (code == 200) {

                                        mostraMensgemOK();
                                        new Handler().postDelayed(new Runnable() {
                                            @Override
                                            public void run() {
                                                fechaCadastro();
                                            }
                                        }, 2000);

                                    } else {
                                        escondeDialog();
                                        mostraMensagem("Esse nome de usuário já está sendo utilizado.");
                                    }
                                }

                                @Override
                                public void onFailure(Call<UsuarioViewModel> call, Throwable t) {
                                    escondeDialog();
                                    mostraMensagem("Algo deu errado, tente novamente.");
                                }
                            });
                    } else {
                        escondeDialog();
                        if (senha1.length() < 4)
                            mostraMensagem("A senha deve possuir pelo menos 4 caracteres");
                        else
                            mostraMensagem("As senhas não correspondem");
                    }
                } else {
                    escondeDialog();
                    if(usuario.indexOf(" ") != -1)
                        mostraMensagem("Nome de Usuario não pode conter espaços");
                    else if(usuario.length() < 3 || nome.length() < 3)
                        mostraMensagem("Nome e Usuario devem possuir pelo menos 3 caracteres.");
                    else
                        mostraMensagem("Insira dados válidos");
                }
            }
        });

        return v;
    }

    public void mostraMensgemOK() {
        escondeDialog();
        scroll.setVisibility(View.GONE);
        cardMensagm.setVisibility(View.VISIBLE);
    }

    public void mostraDialog() {
        dialog = new ProgressDialog(getActivity());
        dialog.setMessage("Aguarde um momento...");
        dialog.setCancelable(false);
        dialog.show();
    }

    public void escondeDialog() {
        if (dialog.isShowing()) {
            dialog.dismiss();
        }
    }


    public void mostraMensagem(String mensagem) {
        txtMensagemErro.setVisibility(View.VISIBLE);
        txtMensagemErro.setText(mensagem);
    }

    public void fechaCadastro() {
        comunicador.layoutIsClosed(true);

        final FragmentManager fm = getActivity().getSupportFragmentManager();
        FragmentTransaction ft = fm.beginTransaction();
        ft.replace(R.id.containerFragment, new Fragment());
        ft.commit();
    }

}
