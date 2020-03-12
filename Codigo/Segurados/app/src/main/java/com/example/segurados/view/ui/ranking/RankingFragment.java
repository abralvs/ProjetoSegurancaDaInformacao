package com.example.segurados.view.ui.ranking;


import android.os.Bundle;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import com.example.segurados.R;
import com.example.segurados.adapter.RankingAdapter;
import com.example.segurados.model.RankingViewModel;
import com.example.segurados.service.UsuarioEstatisticaService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * A simple {@link Fragment} subclass.
 */
public class RankingFragment extends Fragment {
    private RankingAdapter rankingAdapter;
    private RecyclerView mRecyclerView;
    private UsuarioEstatisticaService usuarioEstatisticaService;
    public RankingFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View v =  inflater.inflate(R.layout.fragment_ranking, container, false);
        mRecyclerView = v.findViewById(R.id.rcvRanking);
        RecyclerView.LayoutManager layoutManager = new GridLayoutManager(getActivity(), 2);
        mRecyclerView.setHasFixedSize(true);
        mRecyclerView.setLayoutManager(layoutManager);
        final Call<List<RankingViewModel>> call =  usuarioEstatisticaService.getRanking();
        loadRanking(call);
        return v;
    }

    private void loadRanking(Call<List<RankingViewModel>> call){
        call.enqueue(new Callback<List<RankingViewModel>>() {
            @Override
            public void onResponse(Call<List<RankingViewModel>> call, Response<List<RankingViewModel>> response) {
                int code = response.code();

                if(code == 200){
                    List<RankingViewModel> ranking = response.body();
                    System.out.println(response.body().get(0).getPerfil());
                    rankingAdapter = new RankingAdapter(ranking, getActivity());
                    mRecyclerView.setAdapter(rankingAdapter);
                }else{

                    Toast.makeText(getContext(),"Falhou",
                            Toast.LENGTH_LONG).show();
                }

            }

            @Override
            public void onFailure(Call<List<RankingViewModel>> call, Throwable t) {

            }
        });
    }

}
