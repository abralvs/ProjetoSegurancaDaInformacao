package com.example.segurados.view;

import android.content.Intent;
import android.os.Bundle;
import android.view.WindowManager;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.segurados.R;
import com.example.segurados.model.Usuario;
import com.example.segurados.model.UsuarioViewModel;
import com.example.segurados.service.UsuarioService;
import com.example.segurados.util.Util;

import io.realm.Realm;
import io.realm.RealmResults;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class SplashActivity extends AppCompatActivity {
    private Realm realm;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);

        getSupportActionBar().hide();
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN,
                WindowManager.LayoutParams.FLAG_FULLSCREEN);

        new VerificaUsuario().start();

    }

    public class VerificaUsuario extends Thread {
        private RealmResults<UsuarioViewModel> user;

        @Override
        public void run() {

            realm = Realm.getDefaultInstance();
            user = realm.where(UsuarioViewModel.class).findAll();

            if (user.size() > 0) {

                UsuarioService auth = UsuarioService.retrofit.create(UsuarioService.class);
                final Call<Usuario> call = auth.getUsuario(user.get(0).getIdUsuario(),
                        "bearer " + user.get(0).getToken());

                call.enqueue(new Callback<Usuario>() {
                    @Override
                    public void onResponse(Call<Usuario> call, Response<Usuario> response) {

                        int code = response.code();

                        if (code == 200) {
                           // Toast.makeText(getBaseContext(), "requisicao deu certo",Toast.LENGTH_LONG).show();

                            startActivity(new Intent(SplashActivity.this, MainActivity.class));
                            finish();

                        } else if (code == 400 || code == 401 || code == 403) {
                           // Toast.makeText(getBaseContext(), "Nao autorizado" + code,Toast.LENGTH_LONG).show();

                            Util.removeUser();
                            startActivity(new Intent(SplashActivity.this, LoginActivity.class));
                            finish();
                        }
                    }

                    @Override
                    public void onFailure(Call<Usuario> call, Throwable t) {

                        //Toast.makeText(getBaseContext(), "Algo deu errado. Verifique sua conexao!.",Toast.LENGTH_LONG).show();

                        startActivity(new Intent(SplashActivity.this, MainActivity.class));
                        finish();

                    }
                });

            } else {

                startActivity(new Intent(SplashActivity.this, LoginActivity.class));
                finish();
            }

            realm.close();
        }


    }
}
