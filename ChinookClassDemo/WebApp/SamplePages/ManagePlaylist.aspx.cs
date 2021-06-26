using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.ViewModels;

#endregion

namespace WebApp.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        #region Error Handling
        protected void SelectCheckForException(object sender,
                            ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void InsertCheckForException(object sender,
                    ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been addded");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void UpdateCheckForException(object sender,
                ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been update");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        protected void DeleteCheckForException(object sender,
                ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                MessageUserControl.ShowInfo("Process success", "Album has been removed");
            }
            else
            {
                MessageUserControl.HandleDataBoundException(e);
            }
        }
        #endregion


        protected void ArtistFetch_Click(object sender, EventArgs e)
        {
            
                //code to go here

          }


        protected void GenreFetch_Click(object sender, EventArgs e)
        {

                //code to go here

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {

                //code to go here

        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //code to go here
            //username is coming from the system  via security
            //since security has yet to be installed, a default will be
            //  setup for the username value
            string username = "HansenB";

            //validate data present
            if (string.IsNullOrWhiteSpace(PlaylistName.Text.Trim()))
            {
                MessageUserControl.ShowInfo("Playlist Search",
                    "No playlist name was supplied");
            }
            else
            {
                //do a standard lookup
                //assign results to a gridview
                //use some user friendly error handling
                //the way we are doing the error handling is using
                //  MessageUserControl instead of try/catch
                //MessageUserControl has the try/catch embedded within
                //  the control logic
                //Within the control there exists a method called .TryRun()
                //syntax:
                //  MessageUserControl.TryRun( () => {
                //
                //          your coding logic
                //
                //  }[, "Message title", "success message"]);
                //
                MessageUserControl.TryRun(() =>
                {
                    //your code
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    //the attachment of the playlist to the web control
                    //      will be used throughout the web page; as such
                    //      the code for refreshing is in its own method
                    RefreshPlaylist(sysmgr, username);
                },"Playlist Search","View the requested playlist below.");
            }
        }

        protected void RefreshPlaylist(PlaylistTracksController sysmgr, string username)
        {
            List<UserPlaylistTrack> info = sysmgr.List_TracksForPlaylist(PlaylistName.Text, username);
            PlayList.DataSource = info;
            PlayList.DataBind();
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
 
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            //code to go here
            
        }

    }
}